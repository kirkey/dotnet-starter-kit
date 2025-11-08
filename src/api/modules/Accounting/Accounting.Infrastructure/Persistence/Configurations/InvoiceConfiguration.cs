namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for Invoice entity.
/// </summary>
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices", SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Invoice number - unique
        builder.Property(x => x.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.InvoiceNumber)
            .IsUnique();

        // Name (duplicate of InvoiceNumber for compatibility)
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        // Foreign keys
        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.ConsumptionId);

        // Dates
        builder.Property(x => x.InvoiceDate)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .IsRequired();

        builder.Property(x => x.PaidDate);

        // Amounts
        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PaidAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.UsageCharge)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.BasicServiceCharge)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TaxAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.OtherCharges)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.LateFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.ReconnectionFee)
            .HasPrecision(18, 2);

        builder.Property(x => x.DepositAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DemandCharge)
            .HasPrecision(18, 2);

        builder.Property(x => x.KWhUsed)
            .HasPrecision(18, 2)
            .IsRequired();

        // String properties
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.BillingPeriod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.RateSchedule)
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // Configure relationship to line items (one-to-many)
        builder.HasMany(x => x.LineItems)
            .WithOne()
            .HasForeignKey(li => li.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(Invoice.LineItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Note: Property-level configuration for InvoiceLineItem is handled in InvoiceLineItemConfiguration

        // Indexes for query optimization
        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_Invoices_MemberId");

        builder.HasIndex(x => x.InvoiceDate)
            .HasDatabaseName("IX_Invoices_InvoiceDate");

        builder.HasIndex(x => x.DueDate)
            .HasDatabaseName("IX_Invoices_DueDate");

        builder.HasIndex(x => x.PaidDate)
            .HasDatabaseName("IX_Invoices_PaidDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Invoices_Status");

        builder.HasIndex(x => x.BillingPeriod)
            .HasDatabaseName("IX_Invoices_BillingPeriod");

        builder.HasIndex(x => x.ConsumptionId)
            .HasDatabaseName("IX_Invoices_ConsumptionId");

        // Composite index for member invoices by date
        builder.HasIndex(x => new { x.MemberId, x.InvoiceDate })
            .HasDatabaseName("IX_Invoices_Member_Date");

        // Composite index for unpaid invoices (AR aging reports)
        builder.HasIndex(x => new { x.Status, x.DueDate })
            .HasDatabaseName("IX_Invoices_Status_DueDate");

        // Composite index for member billing period
        builder.HasIndex(x => new { x.MemberId, x.BillingPeriod })
            .HasDatabaseName("IX_Invoices_Member_Period");

        // Composite index for status and member (common queries)
        builder.HasIndex(x => new { x.Status, x.MemberId, x.InvoiceDate })
            .HasDatabaseName("IX_Invoices_Status_Member_Date");

        // Composite index for date range queries with status
        builder.HasIndex(x => new { x.InvoiceDate, x.Status, x.MemberId })
            .HasDatabaseName("IX_Invoices_Date_Status_Member");

        // Composite index for outstanding balance queries
        builder.HasIndex(x => new { x.Status, x.DueDate, x.TotalAmount })
            .HasDatabaseName("IX_Invoices_Status_DueDate_Amount");
    }
}

