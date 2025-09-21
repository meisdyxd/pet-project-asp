using DirectoryService.Application.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using DirectoryService.Domain;

namespace DirectoryService.Infrastructure.Database.Context;

public class DirectoryServiceContext : DbContext, IReadDbContext
{
    public DbSet<Department> Departments { get; set; }
    public IQueryable<Department> DepartmentsRead => Departments.AsNoTracking();
    
    public DbSet<Position> Positions { get; set; }
    public IQueryable<Position> PositionsRead => Positions.AsNoTracking();
    
    public DbSet<Location> Locations { get; set; }
    public IQueryable<Location> LocationsRead => Locations.AsNoTracking();
    public DirectoryServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}