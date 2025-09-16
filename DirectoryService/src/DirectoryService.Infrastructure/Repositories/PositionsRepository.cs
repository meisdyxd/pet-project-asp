using DirectoryService.Application.Interfaces.Database.IRepositories;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects.Position;
using DirectoryService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Position?> GetActiveByNameAsync(Name name, CancellationToken cancellationToken)
    {
        var uniqueWhereActive = await _context.Positions
            .Where(p => p.IsActive)
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);

        return uniqueWhereActive;
    }
}