using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Contracts;
using Dapper;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts.Extensions;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Department;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Application.CQRS.Commands.AddDepartment;

public class AddDepartmentCommandHandler : ICommandHandler<AddDepartmentCommand>
{
    private readonly IDapperConnectionFactory _dapperConnectionFactory;
    private readonly ILogger<AddDepartmentCommandHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly IValidator<AddDepartmentCommand> _validator;

    public AddDepartmentCommandHandler(
        IDapperConnectionFactory dapperConnectionFactory,
        IDepartmentsRepository departmentsRepository,
        IValidator<AddDepartmentCommand> validator,
        ILogger<AddDepartmentCommandHandler> logger)
    {
        _dapperConnectionFactory = dapperConnectionFactory;
        _departmentsRepository = departmentsRepository;
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
        
        using var connection = await _dapperConnectionFactory.CreateConnectionAsync(cancellationToken);
        
        var locationIds = (command.LocationIds ?? []).ToArray();
        const string locationsQuery = "SELECT COUNT(*) FROM locations WHERE id = ANY(@locationIds)";
        var locationParameters = new DynamicParameters();
        locationParameters.Add("locationIds", locationIds);
        int locationCount = await connection.QuerySingleAsync<int>(locationsQuery, locationParameters);
        
        if (locationCount != locationIds.Length)
            return Errors.Http.BadRequestError("Locations not found", "not.found").ToErrorList();
        
        string parentPath = string.Empty;
        if (command.ParentId is not null)
        {
            const string parentQuery = "SELECT path FROM departments WHERE id = @parentId";
            var parentParameters = new DynamicParameters();
            parentParameters.Add("parentId", command.ParentId);
            string? tempPath = await connection.QuerySingleOrDefaultAsync<string>(parentQuery, parentParameters);
            
            if (tempPath is null)
                return Errors.Http.BadRequestError("Parent path not found", "not.found").ToErrorList();

            parentPath = tempPath;
        }
        
        var name = Name.Create(command.Name).Value;
        
        var identifier = Identifier.Create(command.Identifier).Value;
        string separator = parentPath == string.Empty ? string.Empty : ".";
        var path = Path.Create($"{parentPath}{separator}{identifier.Value}").Value;
        
        const string pathQuery = "SELECT COUNT(*) FROM departments WHERE parent_id = @parentId AND path = @path";
        var pathParameters = new DynamicParameters();
        pathParameters.Add("parentId", command.ParentId);
        pathParameters.Add("path", path.Value);
        int pathQueries = await connection.QuerySingleOrDefaultAsync<int>(pathQuery, pathParameters);
        if (pathQueries > 0)
            return Errors.Http.Conflict("Department is already exists", "conflict").ToErrorList();
        
        short depth = (short)path.Value.Count(p => p == '.');
        
        var department = Department.Create(
            name, 
            identifier, 
            path, 
            depth, 
            command.ParentId);
        
        if (department.IsFailure)
        {
            _logger.LogInformation("Failed create a department");
            return department.Error;
        }

        var departmentValue = department.Value;
        departmentValue.AddLocations(locationIds);
        await _departmentsRepository.AddAsync(department.Value, cancellationToken);
        
        var resultSave = await _departmentsRepository.SaveChangesAsync(cancellationToken);
        if (resultSave.IsFailure)
        {
            _logger.LogError("Error when save location to DB");
            return resultSave.Error.ToErrorList();
        }
        
        _logger.LogInformation("Department with ID '{ID}' was successfully created", departmentValue.Id);
        return UnitResult.Success<ErrorList>();
    }
}