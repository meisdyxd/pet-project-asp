using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.Database.IRepositories;

public interface IDepartmentsRepository
{
    Task AddAsync(Department department, CancellationToken cancellationToken);
    Task<Result<string?, ErrorList>> GetParentPathAsync(Guid parentId, CancellationToken cancellationToken);
    Task<bool> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken);
    Task<Department?> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken);
}