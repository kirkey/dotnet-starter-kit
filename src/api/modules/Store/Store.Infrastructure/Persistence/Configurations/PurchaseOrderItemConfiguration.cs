using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        // Ensure required foreign keys and quantities
        builder.Property(x => x.PurchaseOrderId)
            .IsRequired();

        builder.Property(x => x.ItemId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.ReceivedQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for foreign keys
        builder.HasIndex(x => x.PurchaseOrderId)
            .HasDatabaseName("IX_PurchaseOrderItems_PurchaseOrderId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_PurchaseOrderItems_ItemId");

        // Composite index for unique constraint: one line per PO+Item combination
        builder.HasIndex(x => new { x.PurchaseOrderId, x.ItemId })
            .IsUnique()
            .HasDatabaseName("IX_PurchaseOrderItems_PurchaseOrder_Item");

        builder.ToTable("PurchaseOrderItems", SchemaNames.Store);
    }
}
