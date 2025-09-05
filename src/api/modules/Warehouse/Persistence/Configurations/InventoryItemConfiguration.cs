using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems", "warehouse");

        builder.Property(ii => ii.WarehouseId)
            .IsRequired();

        builder.Property(ii => ii.ProductSku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ii => ii.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ii => ii.CurrentStock)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(ii => ii.ReservedStock)
            .IsRequired()
            .HasColumnType("decimal(18,4)")
            .HasDefaultValue(0);

        builder.Property(ii => ii.MinimumStock)
            .IsRequired()
            .HasColumnType("decimal(18,4)")
            .HasDefaultValue(0);

        builder.Property(ii => ii.MaximumStock)
            .IsRequired()
            .HasColumnType("decimal(18,4)")
            .HasDefaultValue(0);

        builder.Property(ii => ii.LastMovementDate)
            .IsRequired();

        // Configure UnitOfMeasure value object
        builder.OwnsOne(ii => ii.UnitOfMeasure, uom =>
        {
            uom.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("UnitOfMeasureName");

            uom.Property(u => u.Symbol)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("UnitOfMeasureSymbol");

            uom.Property(u => u.Description)
                .HasMaxLength(200)
                .HasColumnName("UnitOfMeasureDescription");
        });

        // Computed column for available stock
        builder.Property(ii => ii.AvailableStock)
            .HasComputedColumnSql("([CurrentStock] - [ReservedStock])");

        // Indexes
        builder.HasIndex(ii => ii.WarehouseId);
        builder.HasIndex(ii => ii.ProductSku);
        builder.HasIndex(ii => new { ii.WarehouseId, ii.ProductSku })
            .IsUnique();
        builder.HasIndex(ii => ii.CurrentStock);
        builder.HasIndex(ii => ii.LastMovementDate);
    }
}
