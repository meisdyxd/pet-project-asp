using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.Database.IRepositories;

public interface ILocationsRepository
{
    Task AddAsync(Location location, CancellationToken cancellationToken);
    Task<bool> ExistLocationsAsync(Guid[] locationIds, CancellationToken cancellationToken);
    Task<bool> ExistActiveLocationsAsync(Guid[] locationIds, CancellationToken cancellationToken);
}