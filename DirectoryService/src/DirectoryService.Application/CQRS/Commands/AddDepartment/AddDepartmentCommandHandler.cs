using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Department;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Application.CQRS.Commands.AddDepartment;

public class AddDepartmentCommandHandler : ICommandHandler<AddDepartmentCommand>
{
    private readonly ILogger<AddDepartmentCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<AddDepartmentCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public AddDepartmentCommandHandler(
        IDepartmentsRepository departmentsRepository,
        IReadDbContext readDbContext,
        ITransactionManager transactionManager,
        IValidator<AddDepartmentCommand> validator,
        ILogger<AddDepartmentCommandHandler> logger)
    {
        _departmentsRepository = departmentsRepository;
        _readDbContext = readDbContext;
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
        
        var locationIds = (command.LocationIds ?? []).ToArray();
        var locationCount = await _readDbContext.LocationsRead
            .Where(l => locationIds.Contains(l.Id))
            .CountAsync(cancellationToken);

        if (locationCount != locationIds.Length)
        {
            transaction.Rollback();
            return Errors.Http.BadRequestError("Locations not found", "http.not.found").ToErrorList();
        }
            
        
        var name = Name.Create(command.Name).Value;
        var identifier = Identifier.Create(command.Identifier).Value;
        
        string parentPath = string.Empty;
        if (command.ParentId is not null)
        {
            var parentDepartment = await _readDbContext.DepartmentsRead
                .Where(d => d.Id == command.ParentId)
                .Select(d => new
                {
                    d.Path,
                    IsUniqueIdentifier = d.ChildrenDepartments.All(cd => cd.Identifier.Value != identifier.Value)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (parentDepartment is null)
            {
                transaction.Rollback();
                return Errors.Http.BadRequestError("Parent path not found", "http.not.found").ToErrorList();
            }
            
            if (!parentDepartment.IsUniqueIdentifier)
            {
                transaction.Rollback();
                return Errors.Http.Conflict("Department identifier must be unique", "http.conflict").ToErrorList();
            }
            
            parentPath = parentDepartment.Path.Value;
        }
        
        string separator = parentPath == string.Empty ? string.Empty : ".";
        var path = Path.Create($"{parentPath}{separator}{identifier.Value}").Value;
        short depth = (short)path.Value.Count(p => p == '.');
        
        var department = Department.Create(
            name, 
            identifier, 
            path, 
            depth, 
            command.ParentId);
        
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
            _logger.LogError("Error when saving department to DB: {Error}", resultSave.Error);
            return resultSave.Error;
        }

        var transactionCommit = transaction.Commit();
        if (transactionCommit.IsFailure)
        {
            _logger.LogInformation("Commit transaction failed while add department");
            return transactionCommit.Error;
        }
        
        _logger.LogInformation("Department with ID '{ID}' was successfully created", departmentValue.Id);
        return UnitResult.Success<ErrorList>();
    }
}