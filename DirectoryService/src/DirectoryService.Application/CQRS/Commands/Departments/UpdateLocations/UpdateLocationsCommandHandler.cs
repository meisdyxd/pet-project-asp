using CSharpFunctionalExtensions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Extensions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Commands.Departments.UpdateLocations;

public class UpdateLocationsCommandHandler : ICommandHandler<UpdateLocationsCommand>
{
    private readonly ILogger<UpdateLocationsCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IValidator<UpdateLocationsCommand> _validator;
    
    public UpdateLocationsCommandHandler(
        ILogger<UpdateLocationsCommandHandler> logger,
        IDepartmentsRepository departmentsRepository,
        ILocationsRepository locationsRepository,
        IValidator<UpdateLocationsCommand> validator)
    {
        _logger = logger;
        _validator = validator;
        _departmentsRepository = departmentsRepository;
        _locationsRepository = locationsRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateLocationsCommand command, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var department = await _departmentsRepository.GetByIdAsync(command.DepartmentId, cancellationToken);
        if (department == null)
            return Errors.Http.BadRequestError("Department not found", "http.not.found").ToErrorList();
        if (!department.IsActive)
            return Errors.Http.BadRequestError("Department is non active", "http.logic").ToErrorList();
            
        var activeLocationExists = await _locationsRepository
            .ExistActiveLocationsAsync(command.Request.LocationIds.ToArray(), cancellationToken);
        if (!activeLocationExists)
            return Errors.Http.BadRequestError("Invalid location ids", "http.not.found").ToErrorList();
        
        
        
        return UnitResult.Success<ErrorList>();
    }
}