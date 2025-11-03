using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the GoodsReceipt entity.
/// </summary>
public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
{
    public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.ReceiptNumber)
            .IsUnique();

        builder.Property(x => x.ReceivedDate)
            .IsRequired();

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.WarehouseLocationId)
            .IsRequired(false); // Optional specific location

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        // Notes property is inherited from AuditableEntity and configured in base class

        // Configure relationship to items (one-to-many)
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("GoodsReceiptId")
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(GoodsReceipt.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for query optimization
        builder.HasIndex(x => x.WarehouseId)
            .HasDatabaseName("IX_GoodsReceipts_WarehouseId");

        builder.HasIndex(x => x.PurchaseOrderId)
            .HasDatabaseName("IX_GoodsReceipts_PurchaseOrderId");

        builder.HasIndex(x => x.ReceivedDate)
            .HasDatabaseName("IX_GoodsReceipts_ReceivedDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_GoodsReceipts_Status");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.WarehouseId, x.ReceivedDate })
            .HasDatabaseName("IX_GoodsReceipts_Warehouse_ReceivedDate");

        builder.HasIndex(x => new { x.Status, x.ReceivedDate })
            .HasDatabaseName("IX_GoodsReceipts_Status_ReceivedDate");

        builder.ToTable("GoodsReceipts", SchemaNames.Store);
    }
}

