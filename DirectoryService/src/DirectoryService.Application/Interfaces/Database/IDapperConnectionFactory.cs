using System.Data;

namespace DirectoryService.Application.Interfaces.Database;

public interface IDapperConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}