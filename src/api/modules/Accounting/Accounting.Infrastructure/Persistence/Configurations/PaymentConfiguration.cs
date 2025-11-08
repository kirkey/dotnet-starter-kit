namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Payment entity.
/// Configures database mapping, indexes, and relationships for customer payments.
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    /// <summary>
    /// Configures the Payment entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(Payment), SchemaNames.Accounting);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique payment number (e.g., receipt number)");

        builder.Property(p => p.MemberId)
            .HasComment("Optional member identifier if associated with a specific member");

        builder.Property(p => p.PaymentDate)
            .IsRequired()
            .HasComment("Date the payment was received");

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Total payment amount received; must be positive");

        builder.Property(p => p.UnappliedAmount)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Portion of the payment not yet allocated to invoices");

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Payment method: Cash, Check, EFT, CreditCard");

        builder.Property(p => p.ReferenceNumber)
            .HasMaxLength(100)
            .HasComment("Optional check/reference number");

        builder.Property(p => p.DepositToAccountCode)
            .HasMaxLength(50)
            .HasComment("Optional deposit account code (bank or cash account)");

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .HasComment("Description of the payment");

        builder.Property(p => p.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Configure relationship to allocations (one-to-many)
        builder.HasMany(p => p.Allocations)
            .WithOne()
            .HasForeignKey(a => a.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(Payment.Allocations));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Note: Property-level configuration for PaymentAllocation is handled in PaymentAllocationConfiguration

        // Indexes for performance
        builder.HasIndex(p => p.PaymentNumber)
            .IsUnique()
            .HasDatabaseName("IX_Payment_PaymentNumber");

        builder.HasIndex(p => p.MemberId)
            .HasDatabaseName("IX_Payment_MemberId");

        builder.HasIndex(p => p.PaymentDate)
            .HasDatabaseName("IX_Payment_PaymentDate");

        builder.HasIndex(p => p.PaymentMethod)
            .HasDatabaseName("IX_Payment_PaymentMethod");

        builder.HasIndex(p => p.ReferenceNumber)
            .HasDatabaseName("IX_Payment_ReferenceNumber");

        builder.HasIndex(p => p.DepositToAccountCode)
            .HasDatabaseName("IX_Payment_DepositToAccountCode");

        builder.HasIndex(p => p.UnappliedAmount)
            .HasDatabaseName("IX_Payment_UnappliedAmount");

        // Composite indexes for common query patterns
        builder.HasIndex(p => new { p.MemberId, p.PaymentDate })
            .HasDatabaseName("IX_Payment_Member_PaymentDate");

        builder.HasIndex(p => new { p.PaymentDate, p.PaymentMethod })
            .HasDatabaseName("IX_Payment_PaymentDate_Method");

        builder.HasIndex(p => new { p.PaymentDate, p.Amount })
            .HasDatabaseName("IX_Payment_PaymentDate_Amount");

        // Index for unapplied payments (critical for allocation workflows)
        builder.HasIndex(p => new { p.UnappliedAmount, p.PaymentDate })
            .HasDatabaseName("IX_Payment_UnappliedAmount_PaymentDate");

        // Index for deposit account and date (reconciliation)
        builder.HasIndex(p => new { p.DepositToAccountCode, p.PaymentDate })
            .HasDatabaseName("IX_Payment_DepositAccount_Date");

        // Index for payment method analytics
        builder.HasIndex(p => new { p.PaymentMethod, p.PaymentDate, p.Amount })
            .HasDatabaseName("IX_Payment_Method_Date_Amount");
    }
}

