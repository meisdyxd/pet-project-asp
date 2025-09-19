using DirectoryService.Domain;

namespace DirectoryService.Application.Interfaces.Database;

public interface IReadDbContext
{
    IQueryable<Department> DepartmentsRead { get; }
    IQueryable<Position> PositionsRead { get; }
    IQueryable<Location> LocationsRead { get; }
}