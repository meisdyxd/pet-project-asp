using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

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
}