using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Interfaces.CQRS;
using DirectoryService.Application.Interfaces.Database;
using DirectoryService.Contracts.Dtos.Departments;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Responses.DepartmentResponses;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.CQRS.Queries.Departments.GetWithTopPositions;

public class GetWithTopPositionsQueryHandler : IQueryHandler<GetWithTopPositionsQuery, GetWithTopPositionsResponse>
{
    private readonly ILogger<GetWithTopPositionsQueryHandler> _logger;
    private readonly IDapperConnectionFactory _connectionFactory;

    public GetWithTopPositionsQueryHandler(
        IDapperConnectionFactory connectionFactory, 
        ILogger<GetWithTopPositionsQueryHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task<Result<GetWithTopPositionsResponse, ErrorList>> Handle(
        GetWithTopPositionsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = @"
            SELECT
                d.id,
                d.name,
                d.identifier,
                d.created_at AS ""createdAt"",
                dpa.amount AS ""amountPositions""
            FROM public.departments AS d
            INNER JOIN (
                SELECT
                    department_id,
                    COUNT(*) AS amount,
                    ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC) as rn
                FROM public.department_position
                GROUP BY department_id
            ) AS dpa ON d.id = dpa.department_id
            WHERE dpa.rn <= 5
            ORDER BY dpa.amount DESC;";
        
        var cmd = new CommandDefinition(sql, cancellationToken: cancellationToken);
        try
        {
            var result = await connection.QueryAsync<GetWithTopPositionsDto>(cmd);
            _logger.LogInformation("GetWithTopPositionsQuery executed successfully");
            return new GetWithTopPositionsResponse(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing GetWithTopPositionsQuery with message: {errorMessage}", ex.Message);
            return Errors.DbErrors.Default("Error executing GetWithTopPositionsQuery");
        }
    }
}