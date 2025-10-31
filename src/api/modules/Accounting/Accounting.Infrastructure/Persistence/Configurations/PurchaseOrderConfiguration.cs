namespace Accounting.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.OrderNumber).IsUnique();

        builder.Property(x => x.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.ExpectedDeliveryDate);

        builder.Property(x => x.VendorId)
            .IsRequired();

        builder.Property(x => x.VendorName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ReceivedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.BilledAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.ApprovalStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsFullyReceived)
            .IsRequired();

        builder.Property(x => x.IsFullyBilled)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.RequesterId);

        builder.Property(x => x.RequesterName)
            .HasMaxLength(256);

        builder.Property(x => x.CostCenterId);

        builder.Property(x => x.ProjectId);

        builder.Property(x => x.ShipToAddress)
            .HasMaxLength(1024);

        builder.Property(x => x.PaymentTerms)
            .HasMaxLength(100);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.VendorId);
        builder.HasIndex(x => x.RequesterId);
        builder.HasIndex(x => x.CostCenterId);
        builder.HasIndex(x => x.ProjectId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.OrderDate);
    }
}
