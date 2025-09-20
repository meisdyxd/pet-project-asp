using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Path = DirectoryService.Domain.ValueObjects.Department.Path;

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

    public async Task<Result<Path?, ErrorList>> GetParentPathAsync(Guid parentId, CancellationToken cancellationToken)
    {
        var parentDepartment = await _context.Departments
            .Where(d => d.Id == parentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (parentDepartment is null)
            return Errors.Http.BadRequestError("Parent not found");

        return parentDepartment.Path;
    }

   public async Task<bool> ExistActiveDepartmentsAsync(Guid[] departmentIds, CancellationToken cancellationToken)
    {
        var count = await _context.Departments
            .Where(d => d.IsActive && departmentIds.Contains(d.Id))
            .CountAsync(cancellationToken);

        return count == departmentIds.Length;
    }

    public async Task<Department?> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(d => d.DepartmentLocations)
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);
    }

    public async Task<Result<bool, ErrorList>> CheckTransferable(
        Guid fromId,
        Guid? toId,
        IDbTransaction transaction,
        CancellationToken cancellationToken)
    {
        var connection = transaction.Connection;
        if (connection is null)
            return Errors.DbErrors.Default("Connection is null");

        var dynamicParameters = new DynamicParameters();

        dynamicParameters.AddDynamicParams(new
        {
            ParentId = toId,
            SourceId = fromId
        });

        var cmd = new CommandDefinition(@"
            SELECT NOT(p.""path""::ltree <@ s.""path""::ltree)
            FROM public.departments s
            JOIN public.departments p ON p.id = @ParentId
            WHERE s.id = @SourceId;",
            dynamicParameters,
            transaction: transaction,
            cancellationToken: cancellationToken);

        var isTransferable = await connection.QuerySingleAsync<bool>(cmd);
        return isTransferable;
    }

    public async Task<Result<LockDepartmentDto?, ErrorList>> GetByIdWithLockAsync(
        Guid departmentId, 
        IDbTransaction transaction, 
        CancellationToken cancellationToken)
    {
        var connection = transaction.Connection;
        if (connection is null)
            return Errors.DbErrors.Default("Connection is null");

        var dynamicParameters = new DynamicParameters();

        dynamicParameters.AddDynamicParams(new
        {
            DepartmentId = departmentId
        });

        var cmd = new CommandDefinition(@"
            SELECT src.id, src.""path"", src.is_active, src.parent_id
            FROM public.departments s
            JOIN public.departments src ON src.id = @DepartmentId
            WHERE s.""path""::ltree <@ src.""path""::ltree
            FOR UPDATE OF s;",
            dynamicParameters,
            transaction: transaction,
            cancellationToken: cancellationToken);

        var department = await connection.QueryFirstOrDefaultAsync<LockDepartmentDto?>(cmd);
        return department;
    }

    public async Task<UnitResult<ErrorList>> TransferDepartmentAsync(
        string fromPath, 
        string? toPath, 
        IDbTransaction transaction, 
        CancellationToken cancellationToken)
    {
        var connection = transaction.Connection;
        if (connection is null)
            return Errors.DbErrors.Default("Connection is null");

        var dynamicParameters = new DynamicParameters();

        dynamicParameters.AddDynamicParams(new
        {
            FromPath = fromPath,
            ToPath = toPath ?? string.Empty
        });

        var cmd = new CommandDefinition(@"
            UPDATE departments d
            SET
                path = np.new_path,
                ""depth"" = nlevel(np.new_path)
            FROM (
                SELECT
                    id,
                    @ToPath::ltree || subpath(path::ltree, nlevel(@FromPath::ltree) - 1) AS new_path
                FROM departments
                WHERE path::ltree <@ @FromPath::ltree
            ) AS np
            WHERE d.id = np.id;",
            dynamicParameters,
            transaction: transaction,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(cmd);

        return UnitResult.Success<ErrorList>();
    }
}