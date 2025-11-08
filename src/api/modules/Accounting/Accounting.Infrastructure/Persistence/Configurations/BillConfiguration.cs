namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Bill entity.
/// Defines database schema, constraints, indexes, and relationships.
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills", "accounting");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.VendorId)
            .IsRequired();

        builder.Property(x => x.BillDate)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.IsPosted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.PaidDate)
            .IsRequired(false);

        builder.Property(x => x.PeriodId)
            .IsRequired(false);

        builder.Property(x => x.PaymentTerms)
            .HasMaxLength(100);

        builder.Property(x => x.PurchaseOrderNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Indexes for performance
        builder.HasIndex(x => x.BillNumber)
            .IsUnique()
            .HasDatabaseName("IX_Bills_BillNumber");

        builder.HasIndex(x => x.VendorId)
            .HasDatabaseName("IX_Bills_VendorId");

        builder.HasIndex(x => x.BillDate)
            .HasDatabaseName("IX_Bills_BillDate");

        builder.HasIndex(x => x.DueDate)
            .HasDatabaseName("IX_Bills_DueDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Bills_Status");

        builder.HasIndex(x => x.IsPosted)
            .HasDatabaseName("IX_Bills_IsPosted");

        builder.HasIndex(x => x.IsPaid)
            .HasDatabaseName("IX_Bills_IsPaid");

        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_Bills_PeriodId");

        // Composite indexes for common queries
        builder.HasIndex(x => new { x.VendorId, x.BillDate })
            .HasDatabaseName("IX_Bills_VendorId_BillDate");

        builder.HasIndex(x => new { x.Status, x.DueDate })
            .HasDatabaseName("IX_Bills_Status_DueDate");

        builder.HasIndex(x => new { x.IsPaid, x.DueDate })
            .HasDatabaseName("IX_Bills_IsPaid_DueDate");

        builder.HasIndex(x => new { x.IsPosted, x.BillDate })
            .HasDatabaseName("IX_Bills_IsPosted_BillDate");


        builder.HasIndex(x => new { x.VendorId, x.Status, x.DueDate })
            .HasDatabaseName("IX_Bills_Vendor_Status_DueDate");

        builder.HasIndex(x => new { x.PeriodId, x.BillDate })
            .HasDatabaseName("IX_Bills_Period_BillDate");

        builder.HasIndex(x => new { x.Status, x.DueDate, x.TotalAmount })
            .HasDatabaseName("IX_Bills_Status_DueDate_Amount");

        // Configure one-to-many relationship with BillLineItems
        builder.HasMany(x => x.LineItems)
            .WithOne()
            .HasForeignKey(li => li.BillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

