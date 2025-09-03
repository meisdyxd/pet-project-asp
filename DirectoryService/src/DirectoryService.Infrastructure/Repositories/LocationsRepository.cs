using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
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

    public async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch(Exception ex)
        {
            return Errors.DbErrors.WhenSave(ex.Message);
        }
    }
}