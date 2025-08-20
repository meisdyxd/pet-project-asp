using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Database.Context.Configurations;

public class DepartmentPositionConfiguration: IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_position");
        
        builder.HasKey(d => new  { d.PositionId, d.DepartmentId });
        
        builder.Property(d => d.PositionId)
            .HasColumnName("position_id");
        
        builder.Property(d => d.DepartmentId)
            .HasColumnName("department_id");
        
        builder.HasOne(d => d.Position)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(d => d.PositionId);
        
        builder.HasOne(d => d.Department)
            .WithMany(d => d.DepartmentPositions)
            .HasForeignKey(d => d.DepartmentId);
    }
}