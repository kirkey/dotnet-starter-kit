using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the GoodsReceiptItem entity.
/// </summary>
public class GoodsReceiptItemConfiguration : IEntityTypeConfiguration<GoodsReceiptItem>
{
    public void Configure(EntityTypeBuilder<GoodsReceiptItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GoodsReceiptId)
            .IsRequired();

        builder.Property(x => x.ItemId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitCost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.PurchaseOrderItemId)
            .IsRequired(false); // Optional link for partial receiving

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.GoodsReceiptId)
            .HasDatabaseName("IX_GoodsReceiptItems_GoodsReceiptId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_GoodsReceiptItems_ItemId");

        builder.HasIndex(x => x.PurchaseOrderItemId)
            .HasDatabaseName("IX_GoodsReceiptItems_PurchaseOrderItemId");

        // Composite index for receipt+item reporting
        builder.HasIndex(x => new { x.GoodsReceiptId, x.ItemId })
            .HasDatabaseName("IX_GoodsReceiptItems_Receipt_Item");

        builder.ToTable("GoodsReceiptItems", SchemaNames.Store);
    }
}

