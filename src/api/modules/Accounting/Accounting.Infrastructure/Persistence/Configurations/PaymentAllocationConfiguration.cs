namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PaymentAllocation entity.
/// Maps properties, keys, indexes and relationships for payment allocations.
/// </summary>
public class PaymentAllocationConfiguration : IEntityTypeConfiguration<PaymentAllocation>
{
    public void Configure(EntityTypeBuilder<PaymentAllocation> builder)
    {
        builder.ToTable("PaymentAllocations", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Foreign key to Payment
        builder.Property(x => x.PaymentId)
            .IsRequired();

        // Foreign key to Invoice
        builder.Property(x => x.InvoiceId)
            .IsRequired();

        // Amount allocated
        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Optional notes
        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        // Indexes for query optimization
        
        // Index on PaymentId for foreign key lookups (one-to-many queries)
        builder.HasIndex(x => x.PaymentId)
            .HasDatabaseName("IX_PaymentAllocations_PaymentId");

        // Index on InvoiceId for tracking which payments are applied to an invoice
        builder.HasIndex(x => x.InvoiceId)
            .HasDatabaseName("IX_PaymentAllocations_InvoiceId");

        // Composite index for unique constraint: one allocation per payment-invoice pair
        builder.HasIndex(x => new { x.PaymentId, x.InvoiceId })
            .IsUnique()
            .HasDatabaseName("IX_PaymentAllocations_Payment_Invoice");

        // Composite index for reporting by invoice and amount
        builder.HasIndex(x => new { x.InvoiceId, x.Amount })
            .HasDatabaseName("IX_PaymentAllocations_Invoice_Amount");

        // Relationship to Payment (configured from PaymentConfiguration as well)
        // This ensures the foreign key relationship is properly established
        builder.HasOne<Payment>()
            .WithMany(p => p.Allocations)
            .HasForeignKey(x => x.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship to Invoice for tracking allocations applied to invoices
        builder.HasOne<Invoice>()
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict); // Don't cascade delete if invoice is deleted
    }
}

