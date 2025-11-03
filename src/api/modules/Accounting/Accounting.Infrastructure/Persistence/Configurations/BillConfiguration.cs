namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for Bill entity.
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills", SchemaNames.Accounting);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.BillNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.VendorInvoiceNumber)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(32);
        
        builder.Property(x => x.PaymentTerms)
            .HasMaxLength(100);
        
        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);
        
        builder.Property(x => x.PaymentReference)
            .HasMaxLength(50);
        
        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);
        
        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1000);
        
        builder.Property(x => x.Description)
            .HasMaxLength(2048);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
        
        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.PaidAmount)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.SubtotalAmount)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.TaxAmount)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.ShippingAmount)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 2);
        
        // Indexes
        builder.HasIndex(x => x.BillNumber)
            .IsUnique();
        
        builder.HasIndex(x => x.VendorId);
        
        builder.HasIndex(x => x.BillDate);
        
        builder.HasIndex(x => x.DueDate);
        
        builder.HasIndex(x => x.Status);
        
        // Configure relationship to line items (one-to-many)
        builder.HasMany(x => x.LineItems)
            .WithOne()
            .HasForeignKey(li => li.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(Bill.LineItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Note: Property-level configuration for BillLineItem is handled in BillLineItemConfiguration
    }
}

