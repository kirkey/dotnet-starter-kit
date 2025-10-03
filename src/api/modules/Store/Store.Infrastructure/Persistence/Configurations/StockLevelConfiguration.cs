using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the StockLevel entity.
/// </summary>
public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
{
    public void Configure(EntityTypeBuilder<StockLevel> builder)
    {
        builder.HasKey(x => x.Id);

        // Unique constraint: one stock level per item/warehouse/location/bin combination
        builder.HasIndex(x => new { x.ItemId, x.WarehouseId, x.WarehouseLocationId, x.BinId })
            .IsUnique();

        // Foreign key relationships
        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Bin)
            .WithMany()
            .HasForeignKey(x => x.BinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.LotNumber)
            .WithMany()
            .HasForeignKey(x => x.LotNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.SerialNumber)
            .WithMany()
            .HasForeignKey(x => x.SerialNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.ToTable("StockLevels", SchemaNames.Store);
    }
}
