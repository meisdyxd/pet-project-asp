using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Department;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Application.CQRS.Commands.Departments.AddDepartment;

public class AddDepartmentCommandHandler : ICommandHandler<AddDepartmentCommand>
{
    private readonly ILogger<AddDepartmentCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<AddDepartmentCommand> _validator;

    public AddDepartmentCommandHandler(
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ITransactionManager transactionManager,
        IValidator<AddDepartmentCommand> validator,
        ILogger<AddDepartmentCommandHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        AddDepartmentCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (transactionResult.IsFailure)
            return Errors.DbErrors.BeginTransaction().ToErrorList();
        using var transaction = transactionResult.Value;
        
        var locationIds = (command.Request.LocationIds ?? []).ToArray();
        var existLocations = await _locationsRepository.ExistLocationsAsync(locationIds, cancellationToken);
        if (!existLocations)
        {
            transaction.Rollback();
            return Errors.Http.BadRequestError("Locations not found", "http.not.found").ToErrorList();
        }
        
        var name = Name.Create(command.Request.Name).Value;
        var identifier = Identifier.Create(command.Request.Identifier).Value;
        
        string parentPath = string.Empty;
        if (command.Request.ParentId != null)
        {
            var pathResult = await _departmentsRepository.GetParentPathAsync(
                command.Request.ParentId.Value, 
                command.Request.Identifier, 
                cancellationToken);
            
            if (pathResult.IsFailure)
            {
                transaction.Rollback();
                return pathResult.Error.ToErrorList();
            }
            parentPath = pathResult.Value;
        }

        string separator = parentPath == string.Empty ? string.Empty : ".";
        var path = Path.Create($"{parentPath}{separator}{identifier.Value}").Value;
        short depth = (short)path.Value.Count(p => p == '.');
        
        var department = Department.Create(
            name, 
            identifier, 
            path, 
            depth, 
            command.Request.ParentId);
        
        if (department.IsFailure)
        {
            transaction.Rollback();
            _logger.LogInformation("Failed create a department");
            return department.Error;
        }

        var departmentValue = department.Value;
        departmentValue.AddLocations(locationIds);
        await _departmentsRepository.AddAsync(department.Value, cancellationToken);
        
        var resultSave = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (resultSave.IsFailure)
        {
            transaction.Rollback();
            return resultSave.Error;
        }

        var transactionCommit = transaction.Commit();
        if (transactionCommit.IsFailure)
            return transactionCommit.Error;
        
        _logger.LogInformation("Department with ID '{ID}' was successfully created", departmentValue.Id);
        return UnitResult.Success<ErrorList>();
    }
}