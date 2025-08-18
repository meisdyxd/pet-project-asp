using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Database.Context;

public class DirectoryServiceContext : DbContext
{
    public DirectoryServiceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
