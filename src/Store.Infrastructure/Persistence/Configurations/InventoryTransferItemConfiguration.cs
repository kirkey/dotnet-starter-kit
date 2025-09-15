namespace Store.Infrastructure.Persistence.Configurations;

public class InventoryTransferItemConfiguration : IEntityTypeConfiguration<InventoryTransferItem>
{
    public void Configure(EntityTypeBuilder<InventoryTransferItem> builder)
    {
        builder.HasKey(x => x.Id);

        // Required foreign keys and quantity
        builder.Property(x => x.InventoryTransferId)
            .IsRequired();

        builder.Property(x => x.GroceryItemId)
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

        builder.HasOne(x => x.GroceryItem)
            .WithMany()
            .HasForeignKey(x => x.GroceryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("InventoryTransferItems", SchemaNames.Store);
    }
}
