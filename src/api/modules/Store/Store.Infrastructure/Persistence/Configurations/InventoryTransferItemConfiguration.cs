using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class InventoryTransferItemConfiguration : IEntityTypeConfiguration<InventoryTransferItem>
{
    public void Configure(EntityTypeBuilder<InventoryTransferItem> builder)
    {
        builder.HasKey(x => x.Id);

        // Required foreign keys and quantity
        builder.Property(x => x.InventoryTransferId)
            .IsRequired();

        builder.Property(x => x.ItemId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        // Monetary fields
        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.LineTotal)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.InventoryTransfer)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.InventoryTransferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.InventoryTransferId)
            .HasDatabaseName("IX_InventoryTransferItems_InventoryTransferId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_InventoryTransferItems_ItemId");

        // Composite index for unique constraint: one line per transfer+item
        builder.HasIndex(x => new { x.InventoryTransferId, x.ItemId })
            .IsUnique()
            .HasDatabaseName("IX_InventoryTransferItems_Transfer_Item");

        builder.ToTable("InventoryTransferItems", SchemaNames.Store);
    }
}
