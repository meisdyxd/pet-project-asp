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

        builder.HasIndex(d => d.Path)
            .IsUnique();

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

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

        builder.Property(d => d.ParentId)
            .HasColumnName("parent_id")
            .IsRequired(false);
        
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
            .HasForeignKey(d => d.ParentId);
    }
}