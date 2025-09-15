using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.IRepositories;

public interface ILocationsRepository
{
    Task AddAsync(Location location, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}