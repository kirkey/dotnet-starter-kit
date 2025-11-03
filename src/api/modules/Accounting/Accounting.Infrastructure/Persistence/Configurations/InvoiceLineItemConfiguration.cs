namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InvoiceLineItem entity.
/// Maps properties, keys, indexes and relationships for invoice line items.
/// </summary>
public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.ToTable("InvoiceLineItems", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Foreign key to Invoice
        builder.Property(x => x.InvoiceId)
            .IsRequired();

        // Description
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        // Quantity
        builder.Property(x => x.Quantity)
            .HasPrecision(18, 4)
            .IsRequired();

        // Unit price
        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Total price
        builder.Property(x => x.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Optional account coding
        builder.Property(x => x.AccountId);

        // Indexes for query optimization
        
        // Index on InvoiceId for foreign key lookups (one-to-many queries)
        builder.HasIndex(x => x.InvoiceId)
            .HasDatabaseName("IX_InvoiceLineItems_InvoiceId");

        // Index on AccountId for revenue analysis queries
        builder.HasIndex(x => x.AccountId)
            .HasDatabaseName("IX_InvoiceLineItems_AccountId");

        // Composite index for reporting by invoice and account
        builder.HasIndex(x => new { x.InvoiceId, x.AccountId })
            .HasDatabaseName("IX_InvoiceLineItems_Invoice_Account");

        // Index on Description for text search queries
        builder.HasIndex(x => x.Description)
            .HasDatabaseName("IX_InvoiceLineItems_Description");

        // Relationship to Invoice (configured from InvoiceConfiguration as well)
        // This ensures the foreign key relationship is properly established
        builder.HasOne<Invoice>()
            .WithMany(i => i.LineItems)
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

