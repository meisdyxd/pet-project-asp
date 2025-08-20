using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Database.Context.Configurations;

public class DepartmentLocationConfiguration: IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_location");
        
        builder.HasKey(d => new  { d.LocationId, d.DepartmentId });
        
        builder.Property(d => d.LocationId)
            .HasColumnName("location_id");
        
        builder.Property(d => d.DepartmentId)
            .HasColumnName("department_id");
        
        builder.HasOne(d => d.Location)
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey(d => d.LocationId);
        
        builder.HasOne(d => d.Department)
            .WithMany(d => d.DepartmentLocations)
            .HasForeignKey(d => d.DepartmentId);
    }
}