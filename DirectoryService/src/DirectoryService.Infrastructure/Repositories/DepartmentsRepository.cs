using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
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

    public async Task<Result<string, Error>> GetParentPathAsync(Guid parentId, string identifier, CancellationToken cancellationToken)
    {
        var parentDepartment = await _context.DepartmentsRead
            .AsNoTracking()
            .Where(d => d.Id == parentId)
            .Select(d => new
            {
                d.Path,
                IsUniqueIdentifier = d.ChildrenDepartments.All(cd => cd.Identifier.Value != identifier)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (parentDepartment is null)
            return Errors.Http.BadRequestError("Parent path not found", "http.not.found");

        if (!parentDepartment.IsUniqueIdentifier)
            return Errors.Http.Conflict("Department identifier must be unique", "http.conflict");

        return parentDepartment.Path.Value;
    }

   public async Task<Result<bool, Error>> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken)
    {
        var count = await _context.DepartmentsRead
            .AsNoTracking()
            .Where(d => d.IsActive && departmentIds.Contains(d.Id))
            .CountAsync(cancellationToken);

        if (count != departmentIds.Length)
            return Errors.Http.BadRequestError("Undefined departments", "http.not.found");

        return true;
    }
}