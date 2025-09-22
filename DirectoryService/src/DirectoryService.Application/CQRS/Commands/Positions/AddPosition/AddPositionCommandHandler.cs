using CSharpFunctionalExtensions;
using DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Position;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DirectoryService.Application.CQRS.Commands.Positions.AddPosition;

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
        //Валидация входных данных
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var name = Name.Create(command.Request.Name).Value;
        var description = Description.Create(command.Request.Description).Value;

        //Проверить на существование активной позиции с тем же наименованием
        var activePosition = await _positionsRepository.GetActiveByNameAsync(name, cancellationToken);
        if (activePosition != null)
            return Errors.Http.Conflict("Active position already exist");
        
        //Проверить на существование активных отделов
        var existActiveDepartments = await _departmentsRepository
            .ExistActiveDepartmentsAsync([.. command.Request.DepartmentIds], cancellationToken);
        if (!existActiveDepartments)
            return Errors.Http.UnprocessableContent("Not all departments found");

        //Создание позиции
        var position = Position.Create(name, description);
        if (position.IsFailure)
        {
            _logger.LogInformation("Failed create a department");
            return position.Error;
        }
        
        //Линковка с отделами
        var positionValue = position.Value;
        positionValue.LinkWithDepartments(command.Request.DepartmentIds);
        
        await _positionsRepository.AddAsync(positionValue, cancellationToken);
        
        var resultSave = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (resultSave.IsFailure)
            return resultSave.Error;

        _logger.LogInformation("Position with ID '{ID}' was successfully added.", position.Value.Id);
        return UnitResult.Success<ErrorList>();
    }
}