using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TransactionNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.TransactionNumber)
            .IsUnique();

        builder.Property(x => x.TransactionType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UnitCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Reference)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.PerformedBy)
            .HasMaxLength(100);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(100);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.InventoryTransactions)
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany()
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("InventoryTransactions", SchemaNames.Store);
    }
}
