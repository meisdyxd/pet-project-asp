using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Position;

namespace DirectoryService.Application.Interfaces.IRepositories;

public interface IPositionsRepository
{
    Task AddAsync(Position position, CancellationToken cancellationToken);
    Task<Position?> GetActiveByNameAsync(Name name, CancellationToken cancellationToken);
}