using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TaxAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DiscountAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.NetAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.DeliveryAddress)
            .HasMaxLength(500);

        builder.Property(x => x.ContactPerson)
            .HasMaxLength(100);

        builder.Property(x => x.ContactPhone)
            .HasMaxLength(50);

        builder.HasOne(x => x.Supplier)
            .WithMany(x => x.PurchaseOrders)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship to items (one-to-many)
        builder.HasMany(x => x.Items)
            .WithOne(x => x.PurchaseOrder)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(PurchaseOrder.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.SupplierId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrderDate);
        builder.HasIndex(x => x.ExpectedDeliveryDate);

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.SupplierId, x.OrderDate })
            .HasDatabaseName("IX_PurchaseOrders_Supplier_OrderDate");

        builder.HasIndex(x => new { x.Status, x.OrderDate })
            .HasDatabaseName("IX_PurchaseOrders_Status_OrderDate");

        builder.HasIndex(x => new { x.SupplierId, x.Status })
            .HasDatabaseName("IX_PurchaseOrders_Supplier_Status");

        builder.ToTable("PurchaseOrders", SchemaNames.Store);
    }
}
