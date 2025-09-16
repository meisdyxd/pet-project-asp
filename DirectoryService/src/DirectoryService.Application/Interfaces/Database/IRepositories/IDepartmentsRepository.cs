using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.IRepositories;

public interface IDepartmentsRepository
{
    Task AddAsync(Department department, CancellationToken cancellationToken);
    Task<Result<string, Error>> GetParentPathAsync(Guid parentId, string identifier, CancellationToken cancellationToken);
    Task<Result<bool, Error>> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken);
}