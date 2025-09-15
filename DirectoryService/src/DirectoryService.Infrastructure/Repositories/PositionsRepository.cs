using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces.IRepositories;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Database.Context;

namespace DirectoryService.Infrastructure.Repositories;

public class PositionsRepository : IPositionsRepository
{
    private readonly DirectoryServiceContext  _context;

    public PositionsRepository(DirectoryServiceContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Position position, CancellationToken cancellationToken)
    {
        await _context.AddAsync(position, cancellationToken);
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