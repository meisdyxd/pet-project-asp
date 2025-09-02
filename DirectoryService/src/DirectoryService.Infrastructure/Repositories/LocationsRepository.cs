using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;

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

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}