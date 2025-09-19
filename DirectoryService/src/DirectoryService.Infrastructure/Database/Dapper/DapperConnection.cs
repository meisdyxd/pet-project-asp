using DirectoryService.Application.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using Npgsql;

namespace DirectoryService.Infrastructure.Database.Dapper;

public class DapperConnectionFactory : IDapperConnectionFactory, IDisposable, IAsyncDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger<DapperConnectionFactory> _logger;

    public DapperConnectionFactory(
        IConfiguration configuration,
        ILogger<DapperConnectionFactory> logger)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Database"));
        dataSourceBuilder.UseLoggerFactory(CreateLoggerFactory());
        _dataSource = dataSourceBuilder.Build();
        _logger = logger;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _dataSource.OpenConnectionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not connect to database with Dapper");
            throw;
        }
    }

    private static ILoggerFactory CreateLoggerFactory()
        => LoggerFactory.Create(builder => builder.AddConsole());

    public void Dispose() => _dataSource?.Dispose();

    public async ValueTask DisposeAsync() => await _dataSource.DisposeAsync();
}