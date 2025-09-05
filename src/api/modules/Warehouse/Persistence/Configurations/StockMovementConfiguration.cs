using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements", "warehouse");

        builder.Property(sm => sm.WarehouseId)
            .IsRequired();

        builder.Property(sm => sm.ProductSku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sm => sm.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sm => sm.MovementType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sm => sm.Quantity)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.Property(sm => sm.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(sm => sm.Notes)
            .HasMaxLength(1000);

        builder.Property(sm => sm.MovementDate)
            .IsRequired();

        builder.Property(sm => sm.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(sm => sm.SourceWarehouseId);
        builder.Property(sm => sm.DestinationWarehouseId);

        // Configure UnitOfMeasure value object
        builder.OwnsOne(sm => sm.UnitOfMeasure, uom =>
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

        // Indexes
        builder.HasIndex(sm => sm.WarehouseId);
        builder.HasIndex(sm => sm.ProductSku);
        builder.HasIndex(sm => sm.MovementType);
        builder.HasIndex(sm => sm.Status);
        builder.HasIndex(sm => sm.MovementDate);
        builder.HasIndex(sm => sm.ReferenceNumber);
        builder.HasIndex(sm => new { sm.WarehouseId, sm.ProductSku });
    }
}
