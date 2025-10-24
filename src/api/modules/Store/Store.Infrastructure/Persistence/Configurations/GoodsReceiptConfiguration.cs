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

        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.PurchaseOrderId);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("GoodsReceiptId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}

