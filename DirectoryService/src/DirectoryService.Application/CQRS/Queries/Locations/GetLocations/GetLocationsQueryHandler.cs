using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Enums.LocationEnums;
using DirectoryService.Application.Extensions;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Dtos.Locations;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Responses.LocationResponses;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Queries.Locations.GetLocations;

public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, GetLocationsResponse>
{
    private readonly IDapperConnectionFactory _connectionFactory;
    private readonly IValidator<GetLocationsQuery> _validator;
    private readonly ILogger<GetLocationsQueryHandler> _logger;
    
    public GetLocationsQueryHandler(
        IDapperConnectionFactory connectionFactory,
        IValidator<GetLocationsQuery> validator,
        ILogger<GetLocationsQueryHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<GetLocationsResponse, ErrorList>> Handle(
        GetLocationsQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var querySql = new StringBuilder(@"
            SELECT 
                l.id AS Id,
                l.name AS Name,
                a.Country,
                a.Region,
                a.City,
                a.Street,
                a.HouseNumber,
                a.PostalCode,
                a.District,
                a.Building,
                l.timezone AS Timezone
            FROM locations as l
            CROSS JOIN LATERAL jsonb_to_record(l.address) AS a(
                Country text,
                Region text,
                City text,
                Street text,
                HouseNumber text,
                PostalCode text,
                District text,
                Building text
            )");
        
        var dynamicParameters = new DynamicParameters();
        if (query.DepartmentIds != null && query.DepartmentIds.Any())
        {
            querySql.Append(" INNER JOIN department_location AS d ON d.location_id = l.id AND d.department_id = ANY(@departmentIds)");
            dynamicParameters.Add("@departmentIds", query.DepartmentIds);
        }
        
        var whereClauses = new List<string>();
        
        if (query.Request.IsActive is not null)
        {
            whereClauses.Add("is_active = @IsActive");
            dynamicParameters.Add("IsActive", query.Request.IsActive);
        }

        if (query.Request.Search is not null)
        {
            whereClauses.Add("name LIKE @Search");
            dynamicParameters.Add("Search", $"%{query.Request.Search}%");
        }

        if (whereClauses.Count != 0)
        {
            querySql.Append(" WHERE ");
            querySql.Append(string.Join(" AND ", whereClauses));
        }
        var orderByClauses = new List<string>();
        var direction = query.Request.SortDirection?.ToUpper() ?? "ASC";
        if (query.Request.SortBy is not null)
        {
            foreach (var sortBy in query.Request.SortBy)
            {
                var sortString = Enum.Parse<GetLocationsSortFields>(sortBy) switch
                {
                    GetLocationsSortFields.NAME => "l.name",
                    GetLocationsSortFields.CREATED_AT => "l.created_at",
                    _ => throw new ArgumentException("Undefined sortBy field")
                };
                orderByClauses.Add($"{sortString} {direction}");
            }
        }
        
        querySql.Append(" ORDER BY ");
        if (orderByClauses.Count != 0)
            querySql.Append(string.Join(", ", orderByClauses));
        else
            querySql.Append($"l.name {direction}");
        
        var withQuerySql = new StringBuilder(@$"
            WITH flat_locations AS({querySql})
            SELECT *, COUNT(*) OVER () AS ""total_count"" FROM flat_locations OFFSET @Offset LIMIT @Limit");
        
        dynamicParameters.Add("@Offset", (query.Request.Page-1)*query.Request.PageSize);
        dynamicParameters.Add("@Limit", query.Request.PageSize);

        long totalCount = 0;
        try
        {
            var result = await connection.QueryAsync<GetLocationDto, long, GetLocationDto>(
                withQuerySql.ToString(),
                splitOn: "total_count",
                map: (dto, l) =>
                {
                    totalCount = l;
                    return dto;
                }, 
                param: dynamicParameters);
        
            return new GetLocationsResponse(
                [.. result],
                query.Request.Page,
                query.Request.PageSize,
                totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "While get locations with message: {message}", ex.Message);
            return Errors.DbErrors.Default(ex.Message);
        }
    }
}