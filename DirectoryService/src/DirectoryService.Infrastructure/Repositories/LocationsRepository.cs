using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using DirectoryService.Domain;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository : ILocationsRepository
{
    private readonly DirectoryServiceContext _context;
    
    public LocationsRepository(DirectoryServiceContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Location location, CancellationToken cancellationToken)
    {
        await _context.AddAsync(location, cancellationToken);
    }

    public async Task<bool> ExistLocationsAsync(Guid[] locationIds, CancellationToken cancellationToken)
    {
        var locationCount = await _context.Locations
            .Where(l => locationIds.Contains(l.Id))
            .CountAsync(cancellationToken);

        return locationCount == locationIds.Length;
    }
    
    public async Task<bool> ExistActiveLocationsAsync(Guid[] locationIds, CancellationToken cancellationToken)
    {
        var locationCount = await _context.Locations
            .Where(l => l.IsActive)
            .Where(l => locationIds.Contains(l.Id))
            .CountAsync(cancellationToken);

        return locationCount == locationIds.Length;
    }
}