using DirectoryService.Contracts;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Database.Context.Configurations;

public class PositionConfiguration: IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");

        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.ComplexProperty(d => d.Name, np =>
        {
            np.Property(p => p.Value)
                .HasMaxLength(Constants.PositionConstants.MAX_LENGTH_NAME)
                .HasColumnName("name")
                .IsRequired();
        });

        builder.ComplexProperty(d => d.Description, dp =>
        {
            dp.Property(p => p.Value)
                .HasMaxLength(Constants.PositionConstants.MAX_LENGTH_DESCRIPTION)
                .HasColumnName("description")
                .IsRequired();
        });

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
    }
}