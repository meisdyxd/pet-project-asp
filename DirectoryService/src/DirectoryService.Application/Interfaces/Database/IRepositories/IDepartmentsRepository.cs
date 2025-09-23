using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;
using System.Data;
using DirectoryService.Contracts.Dtos.Departments;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

namespace DirectoryService.Application.Interfaces.Database.IRepositories;

public interface IDepartmentsRepository
{
    Task AddAsync(Department department, CancellationToken cancellationToken);
    Task<Result<Path?, ErrorList>> GetParentPathAsync(Guid parentId, CancellationToken cancellationToken);
    Task<bool> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken);
    Task<Department?> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken);
    Task<Result<bool, ErrorList>> CheckTransferable(Guid fromId, Guid? toId, IDbTransaction transaction, CancellationToken cancellationToken);
    Task<Result<LockDepartmentDto?, ErrorList>> GetByIdWithLockAsync(Guid departmentId, IDbTransaction transaction, CancellationToken cancellationToken);
    Task<UnitResult<ErrorList>> TransferDepartmentAsync(string fromPath, string? toPath, IDbTransaction transaction, CancellationToken cancellationToken);
    Task UpdateParentAsync(Guid departmentId, Guid? parentId, CancellationToken cancellationToken);
}