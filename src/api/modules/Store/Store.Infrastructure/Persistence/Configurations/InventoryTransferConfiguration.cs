using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class InventoryTransferConfiguration : IEntityTypeConfiguration<InventoryTransfer>
{
    public void Configure(EntityTypeBuilder<InventoryTransfer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TransferNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.TransferNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TransferType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Priority)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.TotalValue)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TransportMethod)
            .HasMaxLength(100);

        builder.Property(x => x.TrackingNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.RequestedBy)
            .HasMaxLength(100);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(100);

        builder.HasOne(x => x.FromWarehouse)
            .WithMany()
            .HasForeignKey(x => x.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToWarehouse)
            .WithMany()
            .HasForeignKey(x => x.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromLocation)
            .WithMany()
            .HasForeignKey(x => x.FromLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ToLocation)
            .WithMany()
            .HasForeignKey(x => x.ToLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure relationship to items (one-to-many)
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.InventoryTransferId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(InventoryTransfer.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.FromWarehouseId)
            .HasDatabaseName("IX_InventoryTransfers_FromWarehouseId");

        builder.HasIndex(x => x.ToWarehouseId)
            .HasDatabaseName("IX_InventoryTransfers_ToWarehouseId");

        builder.HasIndex(x => x.FromLocationId)
            .HasDatabaseName("IX_InventoryTransfers_FromLocationId");

        builder.HasIndex(x => x.ToLocationId)
            .HasDatabaseName("IX_InventoryTransfers_ToLocationId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_InventoryTransfers_Status");

        builder.HasIndex(x => x.TransferType)
            .HasDatabaseName("IX_InventoryTransfers_TransferType");

        builder.HasIndex(x => x.TransferDate)
            .HasDatabaseName("IX_InventoryTransfers_TransferDate");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.FromWarehouseId, x.TransferDate })
            .HasDatabaseName("IX_InventoryTransfers_FromWarehouse_TransferDate");

        builder.HasIndex(x => new { x.ToWarehouseId, x.TransferDate })
            .HasDatabaseName("IX_InventoryTransfers_ToWarehouse_TransferDate");

        builder.HasIndex(x => new { x.Status, x.TransferDate })
            .HasDatabaseName("IX_InventoryTransfers_Status_TransferDate");

        builder.HasIndex(x => new { x.FromWarehouseId, x.ToWarehouseId })
            .HasDatabaseName("IX_InventoryTransfers_FromWarehouse_ToWarehouse");

        builder.ToTable("InventoryTransfers", SchemaNames.Store);
    }
}
