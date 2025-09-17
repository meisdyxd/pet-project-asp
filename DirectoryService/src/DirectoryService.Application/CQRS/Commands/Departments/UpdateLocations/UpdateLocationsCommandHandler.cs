using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;

public class UpdateLocationsCommandHandler : ICommandHandler<UpdateLocationsCommand>
{
    private readonly ILogger<UpdateLocationsCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<UpdateLocationsCommand> _validator;
    private readonly ITransactionManager _transactionManager;
    
    public UpdateLocationsCommandHandler(
        ILogger<UpdateLocationsCommandHandler> logger,
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        ITransactionManager transactionManager,
        IValidator<UpdateLocationsCommand> validator)
    {
        _logger = logger;
        _validator = validator;
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateLocationsCommand command, 
        CancellationToken cancellationToken)
    {
        //Валидация входных данных
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        //Проверить — существует ли подразделение с таким departmentId и оно активно
        var department = await _departmentsRepository.GetByIdAsync(command.DepartmentId, cancellationToken);
        if (department == null)
            return Errors.Http.NotFound("Department not found");
        if (!department.IsActive)
            return Errors.Http.BadRequestError("Department is non active");
        
        //Проверить — все locationIds существуют и активны, нет дубликатов
        var activeLocationExists = await _locationsRepository
            .ExistActiveLocationsAsync([.. command.Request.LocationIds], cancellationToken);
        if (!activeLocationExists)
            return Errors.Http.UnprocessableContent("Invalid location ids");
        
        //Обновить — заменить старые привязки к локациям новым списком
        department.UpdateLocations(command.Request.LocationIds);
        var resultSave = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (resultSave.IsFailure)
            return resultSave.Error;
        
        return UnitResult.Success<ErrorList>();
    }
}