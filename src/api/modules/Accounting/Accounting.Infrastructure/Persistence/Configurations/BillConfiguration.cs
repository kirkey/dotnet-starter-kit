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
        
        // Owned collection for line items
        builder.OwnsMany(x => x.LineItems, lineItem =>
        {
            lineItem.ToTable("BillLineItems", SchemaNames.Accounting);
            lineItem.WithOwner().HasForeignKey("BillId");
            lineItem.Property<int>("Id");
            lineItem.HasKey("Id");
            
            lineItem.Property(li => li.Description)
                .IsRequired()
                .HasMaxLength(500);
            
            lineItem.Property(li => li.Quantity)
                .HasPrecision(18, 4);
            
            lineItem.Property(li => li.UnitPrice)
                .HasPrecision(18, 2);
            
            lineItem.Property(li => li.LineTotal)
                .HasPrecision(18, 2);
        });
    }
}

