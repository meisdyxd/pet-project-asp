using System.Data;

namespace DirectoryService.Application.Interfaces;

public interface IDapperConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}