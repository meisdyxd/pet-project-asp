using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;

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