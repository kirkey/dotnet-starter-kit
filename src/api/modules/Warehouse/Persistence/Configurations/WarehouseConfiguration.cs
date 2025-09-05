using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal class WarehouseConfiguration : IEntityTypeConfiguration <Domain.Warehouse>
{
    public void Configure(EntityTypeBuilder<Domain.Warehouse> builder)
    {
        builder.ToTable("Warehouses", "warehouse");

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Description)
            .HasMaxLength(1000);

        builder.Property(w => w.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Configure Address value object
        builder.OwnsOne(w => w.Address, a =>
        {
            a.Property(addr => addr.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("AddressStreet");

            a.Property(addr => addr.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressCity");

            a.Property(addr => addr.State)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressState");

            a.Property(addr => addr.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("AddressPostalCode");

            a.Property(addr => addr.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("AddressCountry");
        });

        // Configure Capacity value object
        builder.OwnsOne(w => w.Capacity, c =>
        {
            c.Property(cap => cap.MaxWeight)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("CapacityMaxWeight");

            c.Property(cap => cap.MaxVolume)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("CapacityMaxVolume");

            c.Property(cap => cap.WeightUnit)
                .HasMaxLength(10)
                .HasColumnName("CapacityWeightUnit");

            c.Property(cap => cap.VolumeUnit)
                .HasMaxLength(10)
                .HasColumnName("CapacityVolumeUnit");
        });

        // Indexes
        builder.HasIndex(w => w.Code)
            .IsUnique();

        builder.HasIndex(w => w.Name);
        builder.HasIndex(w => w.IsActive);
    }
}
