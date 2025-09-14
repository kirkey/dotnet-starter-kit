using Shared.Constants;

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

        builder.ToTable("InventoryTransfers", SchemaNames.Store);
    }
}
