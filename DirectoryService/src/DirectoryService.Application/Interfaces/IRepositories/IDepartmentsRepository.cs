using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.IRepositories;

public interface IDepartmentsRepository
{
    Task AddAsync(Department department, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}