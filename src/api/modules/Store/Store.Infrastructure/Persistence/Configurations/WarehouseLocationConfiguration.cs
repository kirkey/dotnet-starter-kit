using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

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
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(x => x.UsedCapacity)
            .HasColumnType("decimal(18,3)")
            .IsRequired()
            .HasDefaultValue(0m);

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

        // Table-level constraints: capacity positive and used within range; temperature constraints when required
        // builder.ToTable("WarehouseLocations", SchemaNames.Store, tb =>
        // {
        //     tb.HasCheckConstraint("CK_WarehouseLocations_Capacity_Positive", "Capacity > 0");
        //     tb.HasCheckConstraint("CK_WarehouseLocations_UsedCapacity_Range", "UsedCapacity >= 0 AND UsedCapacity <= Capacity");
        //     tb.HasCheckConstraint("CK_WarehouseLocations_TemperatureConstraints", "RequiresTemperatureControl = false OR (MinTemperature IS NOT NULL AND MaxTemperature IS NOT NULL AND MaxTemperature > MinTemperature)");
        // });
    }
}
