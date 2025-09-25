using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.CQRS.Queries.Departments.GetWithTopPositions;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Contracts.Dtos.Departments;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Responses.DepartmentResponses;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Queries.Departments.GetChildDepartments;

public class GetChildDepartmentsQueryHandler 
    : IQueryHandler<GetChildDepartmentsQuery, GetChildDepartmentsResponse>
{
    private readonly ILogger<GetWithTopPositionsQueryHandler> _logger;
    private readonly IDapperConnectionFactory _connectionFactory;
    private readonly IValidator<GetChildDepartmentsQuery> _validator;

    public GetChildDepartmentsQueryHandler(
        IDapperConnectionFactory connectionFactory, 
        ILogger<GetWithTopPositionsQueryHandler> logger, 
        IValidator<GetChildDepartmentsQuery> validator)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<GetChildDepartmentsResponse, ErrorList>> Handle(
        GetChildDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        const string sql = @"WITH child_departments AS (
    SELECT 
        d.id, 
        d.name,
        d.identifier,
        d.parent_id AS ""parentId"",
        d.created_at AS ""createdAt"",
        COUNT(*) OVER () AS ""totalCount"" 
    FROM departments d
    WHERE parent_id = @ParentId
    ORDER BY d.created_at
    OFFSET @Offset LIMIT @Limit
)
SELECT 
	id,
	""name"",
	identifier,
	""parentId"",
	""createdAt"", 
	EXISTS(
		SELECT 1 
		FROM departments d 
		WHERE d.parent_id = child_departments.id 
		LIMIT 1
	) AS ""hasMoreChildren"",
	""totalCount""
FROM child_departments;";

        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("ParentId", query.ParentId);
        dynamicParameters.Add("Offset", (query.Page - 1) * query.PageSize);
        dynamicParameters.Add("Limit", query.PageSize);

        long totalCount = 0;

        try
        {
            var result = await connection
                .QueryAsync<AcrossDepartmentDto, long, AcrossDepartmentDto>(
                    sql,
                    splitOn: "totalCount",
                    map: (dto, l) =>
                    {
                        totalCount = l;
                        return dto;
                    },
                    param: dynamicParameters
                );
            
            _logger.LogInformation("GetChildDepartmentsQuery was successfully executed.");
            var responseDepartments = result.Select(d =>
                new GetChildDepartmentsDto(d.Id, d.Name, d.Identifier, d.CreatedAt, d.HasMoreChildren));
            return new GetChildDepartmentsResponse(responseDepartments, query.Page, query.PageSize, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Errors.DbErrors.Default("Error executing GetChildDepartmentsQuery");
        }
    }
}