using CSharpFunctionalExtensions;
using DirectoryService.Application.CQRS.Commands.AddDepartment;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Position;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Commands.AddPosition;

public class AddPositionCommandHandler : ICommandHandler<AddPositionCommand>
{
    private readonly ILogger<AddDepartmentCommandHandler> _logger;
    private readonly IPositionsRepository _positionsRepository;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<AddPositionCommand> _validator;

    public AddPositionCommandHandler(
        IPositionsRepository positionsRepository,
        IDepartmentsRepository departmentsRepository,
        IValidator<AddPositionCommand> validator,
        ITransactionManager transactionManager,
        ILogger<AddDepartmentCommandHandler> logger)
    {
        _positionsRepository = positionsRepository;
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        AddPositionCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (transactionResult.IsFailure)
            return Errors.DbErrors.BeginTransaction().ToErrorList();
        using var transaction = transactionResult.Value;

        var activePosition = await _positionsRepository.GetActiveByNameAsync(name, cancellationToken);

        if (activePosition != null)
        {
            transaction.Rollback();
            return Errors.Http.Conflict("Active position already exists", "http.conflict").ToErrorList();
        }
        
        var existActiveDepartmentsResult = await _departmentsRepository
            .ExistActiveDepartmentsAsync([.. command.DepartmentIds], cancellationToken);

        var position = Position.Create(name, description);
        if (position.IsFailure)
        {
            transaction.Rollback();
            _logger.LogInformation("Failed create a department");
            return position.Error;
        }
        
        var positionValue = position.Value;
        positionValue.LinkWithDepartments(command.DepartmentIds);
        
        await _positionsRepository.AddAsync(positionValue, cancellationToken);
        
        var resultSave = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (resultSave.IsFailure)
        {
            transaction.Rollback();
            return resultSave.Error;
        }
        
        var transactionCommit = transaction.Commit();
        if (transactionCommit.IsFailure)
            return transactionCommit.Error;
        
        return UnitResult.Success<ErrorList>();
    }
}