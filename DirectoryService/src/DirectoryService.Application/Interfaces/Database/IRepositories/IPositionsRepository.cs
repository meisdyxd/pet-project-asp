using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.IRepositories;

public interface IPositionsRepository
{
    Task AddAsync(Position position, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}