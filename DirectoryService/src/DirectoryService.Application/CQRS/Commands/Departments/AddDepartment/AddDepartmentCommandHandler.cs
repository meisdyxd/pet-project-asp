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
using System.Net;
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
        //Валидация входных данных
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        //Транзакция
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        if (transactionResult.IsFailure)
            return Errors.DbErrors.BeginTransaction();
        using var transaction = transactionResult.Value;
        
        //Проверка существованяи локаций
        var locationIds = (command.Request.LocationIds ?? []).ToArray();
        var existLocations = await _locationsRepository.ExistLocationsAsync(locationIds, cancellationToken);
        if (!existLocations)
        {
            transaction.Rollback();
            return Errors.Http.UnprocessableContent("Not all locations found");
        }
        
        var name = Name.Create(command.Request.Name).Value;
        var identifier = Identifier.Create(command.Request.Identifier).Value;
        
        //Получение родительского пути, для построения нового
        string? parentPath = null;
        if (command.Request.ParentId != null)
        {
            var parentPathResult = await _departmentsRepository.GetParentPathAsync(
                command.Request.ParentId.Value,
                cancellationToken);
            
            if (parentPathResult.IsFailure)
            {
                transaction.Rollback();
                return parentPathResult.Error;
            }
            parentPath = parentPathResult.Value;
        }
        
        //Построение нового пути
        string separator = parentPath is null ? string.Empty : ".";
        var pathResult = Path.Create($"{parentPath}{separator}{identifier.Value}");
        if (pathResult.IsFailure)
            return pathResult.Error.ToErrorList();

        var path = pathResult.Value;
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

        //Добавление локаций
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