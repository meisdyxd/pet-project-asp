using DirectoryService.Contracts;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Database.Context.Configurations;

public class DepartmentConfiguration: IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");

        builder.ComplexProperty(d => d.Name, np =>
        {
            np.Property(p => p.Value)
                .HasMaxLength(Constants.DepartmentConstants.MAX_LENGTH_NAME)
                .HasColumnName("name")
                .IsRequired();
        });
        
        builder.ComplexProperty(d => d.Identifier, ip =>
        {
            ip.Property(p => p.Value)
                .HasMaxLength(Constants.DepartmentConstants.MAX_LENGTH_IDENTIFIER)
                .HasColumnName("identifier")
                .IsRequired();
        });
        
        builder.ComplexProperty(d => d.Path, pp =>
        {
            pp.Property(p => p.Value)
                .HasColumnName("path")
                .IsRequired();
        });

        builder.Property(d => d.Depth)
            .HasColumnName("depth")
            .IsRequired();
        
        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(Constants.CommonConstants.IS_ACTIVE_DEFAULT)
            .IsRequired();
        
        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasMany(d => d.ChildrenDepartments)
            .WithOne(cd => cd.Parent)
            .HasForeignKey("parent_id");

        builder.HasMany(d => d.Locations)
            .WithMany(l => l.Departments)
            .UsingEntity<DepartmentLocation>(
            r => r.HasOne(dl => dl.Location)
                .WithMany()
                .HasForeignKey(dl => dl.LocationId),
            l => l.HasOne(dl => dl.Department)
                .WithMany()
                .HasForeignKey(dl => dl.DepartmentId),
            j =>
            {
                j.ToTable("department_location");

                j.HasKey(dl => new { dl.DepartmentId, dl.LocationId });

                j.Property(dl => dl.DepartmentId)
                    .HasColumnName("department_id");

                j.Property(dl => dl.LocationId)
                    .HasColumnName("location_id");
            });

        builder.HasMany(d => d.Positions)
            .WithMany(p => p.Departments)
            .UsingEntity<DepartmentPosition>(
            r => r.HasOne(dp => dp.Position)
                .WithMany()
                .HasForeignKey(dp => dp.PositionId),
            l => l.HasOne(dp => dp.Department)
                .WithMany()
                .HasForeignKey(dp => dp.DepartmentId),
            j =>
            {
                j.ToTable("department_position");

                j.Property(dp => dp.PositionId)
                    .HasColumnName("position_id");

                j.Property(dp => dp.DepartmentId)
                    .HasColumnName("department_id");
            });
    }
}