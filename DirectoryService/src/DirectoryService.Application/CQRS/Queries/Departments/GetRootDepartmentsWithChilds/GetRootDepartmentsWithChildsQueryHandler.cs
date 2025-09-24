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

namespace DirectoryService.Application.CQRS.Queries.Departments.GetRootDepartmentsWithChilds;

public class GetRootDepartmentsWithChildsQueryHandler 
    : IQueryHandler<GetRootDepartmentsWithChildsQuery, GetRootDepartmentsWithChildsResponse>
{
    private readonly ILogger<GetWithTopPositionsQueryHandler> _logger;
    private readonly IDapperConnectionFactory _connectionFactory;
    private readonly IValidator<GetRootDepartmentsWithChildsQuery> _validator;

    public GetRootDepartmentsWithChildsQueryHandler(
        IDapperConnectionFactory connectionFactory, 
        ILogger<GetWithTopPositionsQueryHandler> logger, 
        IValidator<GetRootDepartmentsWithChildsQuery> validator)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<GetRootDepartmentsWithChildsResponse, ErrorList>> Handle(
        GetRootDepartmentsWithChildsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        const string sql = @"WITH roots AS (
    SELECT 
        d.id, 
        d.name,
        d.identifier,
        d.parent_id AS ""parentId"",
        d.created_at AS ""createdAt"",
        COUNT(*) OVER () AS ""totalCount""
    FROM departments d
    WHERE parent_id IS NULL
    ORDER BY d.created_at
    OFFSET @Offset LIMIT @Limit
)
(
	SELECT 
		id,
		""name"",
		identifier,
		""parentId"",
		""createdAt"", 
		EXISTS(
			SELECT 1 
			FROM departments d 
			WHERE d.parent_id = roots.id 
			OFFSET @Prefetch LIMIT 1
		) AS ""hasMoreChildren"",
		""totalCount""
	FROM roots
)        
UNION ALL
(
SELECT 
	c.id,
	c.""name"",
	c.identifier,
	c.parent_id,
	c.created_at,
	FALSE ""hasMoreChildren"",
	c.""totalCount""
FROM roots
CROSS JOIN LATERAL (
    SELECT
    	d.id,
        d.name,
        d.identifier,
        d.parent_id,
        d.created_at,
        0 ""totalCount""
    FROM departments d
    WHERE d.parent_id = roots.id
    LIMIT @Prefetch
    ) AS c
);";

        var dynamicParameters = new DynamicParameters();
        
        dynamicParameters.Add("Prefetch", query.Prefetch);
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
                        if (dto.ParentId is null)
                            totalCount = l;
                        return dto;
                    },
                    param: dynamicParameters
                );
            
            _logger.LogInformation("GetRootDepartmentsWithChilds was successfully executed.");
            return new GetRootDepartmentsWithChildsResponse(ComputeResponse(result), query.Page, query.PageSize, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Errors.DbErrors.Default("Error executing GetRootDepartmentsWithChilds");
        }
    }

    private IEnumerable<GetRootDepartmentsDto> ComputeResponse(IEnumerable<AcrossDepartmentDto> departments)
    {
        var childs = new Dictionary<Guid, List<ChildDepartmentDto>>();
        var roots = new Dictionary<Guid, GetRootDepartmentsDto>();

        foreach (var tempDepartment in departments)
        {
            if (tempDepartment.ParentId is null)
            {
                roots[tempDepartment.Id] = new GetRootDepartmentsDto(
                    tempDepartment.Id,
                    tempDepartment.Name, 
                    tempDepartment.Identifier, 
                    tempDepartment.CreatedAt, 
                    tempDepartment.HasMoreChildren,
                    []);
                if (childs.TryGetValue(tempDepartment.Id, out var childrens))
                    roots[tempDepartment.Id].Childs.AddRange(childrens);
            }
            else
            {
                var child = new ChildDepartmentDto(
                    tempDepartment.Id, 
                    tempDepartment.Name, 
                    tempDepartment.Identifier,
                    tempDepartment.CreatedAt);
                
                if (!roots.TryGetValue(tempDepartment.ParentId.Value, out var root))
                {
                    if (!childs.TryGetValue(tempDepartment.ParentId.Value, out var lstChilds))
                    {
                        lstChilds = [];
                        childs[tempDepartment.ParentId.Value] = lstChilds;
                    }

                    lstChilds.Add(child);   
                }
                else
                    root.Childs.Add(child);
            }
        }
        
        return roots.Values;
    }
}