using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DirectoryServiceContext _context;

    public DepartmentsRepository(
        DirectoryServiceContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.AddAsync(department, cancellationToken);
    }

    public async Task<Result<string?, Error>> GetParentPathAsync(Guid parentId, string identifier, CancellationToken cancellationToken)
    {
        var parentDepartment = await _context.Departments
            .Where(d => d.Id == parentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (parentDepartment is null)
            return Errors.Http.BadRequestError("Parent not found", "http.not.found");

        return parentDepartment.Path.Value;
    }

   public async Task<bool> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken)
    {
        var count = await _context.Departments
            .Where(d => d.IsActive && departmentIds.Contains(d.Id))
            .CountAsync(cancellationToken);

        if (count != departmentIds.Length)
            return false;

        return true;
    }

    public async Task<Department?> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(d => d.DepartmentLocations)
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);
    }
}