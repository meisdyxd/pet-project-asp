using DirectoryService.Contracts;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Database.Context.Configurations;

public class LocationConfiguration: IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id");

        builder.ComplexProperty(l => l.Name, np =>
        {
            np.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.LocationConstants.MAX_LENGTH_NAME)
                .HasColumnName("name");
        });

        builder.OwnsOne(l => l.Address, o =>
        {
            o.ToJson("address");

            o.Property(p => p.Country)
                .HasJsonPropertyName("country");

            o.Property(p => p.Region)
                .HasJsonPropertyName("region");

            o.Property(p => p.City)
                .HasJsonPropertyName("city");

            o.Property(p => p.Street)
                .HasJsonPropertyName("street");

            o.Property(p => p.HouseNumber)
                .HasJsonPropertyName("house_number");

            o.Property(p => p.PostalCode)
                .IsRequired(false)
                .HasJsonPropertyName("postal_code");

            o.Property(p => p.District)
                .IsRequired(false)
                .HasJsonPropertyName("district");

            o.Property(p => p.Building)
                .IsRequired(false)
                .HasJsonPropertyName("building");

            o.Property(p => p.Apartment)
                .IsRequired(false)
                .HasJsonPropertyName("apartment");
        });

        builder.ComplexProperty(p => p.Timezone, tp =>
        {
            tp.Property(p => p.Value)
                .HasColumnName("timezone");
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