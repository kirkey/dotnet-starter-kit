using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain;

namespace Store.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.Property(x => x.ManagerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ManagerEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ManagerPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TotalCapacity)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.UsedCapacity)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.CapacityUnit)
            .IsRequired()
            .HasMaxLength(20);

        builder.ToTable("Warehouses", "Store");
    }
}

public class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Aisle)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Section)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Shelf)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Bin)
            .HasMaxLength(20);

        builder.Property(x => x.LocationType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Capacity)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.UsedCapacity)
            .HasColumnType("decimal(18,3)");

        builder.Property(x => x.CapacityUnit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.MinTemperature)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.MaxTemperature)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.TemperatureUnit)
            .HasMaxLength(10);

        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.Locations)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("WarehouseLocations", "Store");
    }
}
