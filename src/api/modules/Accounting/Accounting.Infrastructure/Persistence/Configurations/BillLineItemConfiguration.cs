namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for BillLineItem entity.
/// Maps properties, keys, indexes and relationships for bill line items.
/// </summary>
public class BillLineItemConfiguration : IEntityTypeConfiguration<BillLineItem>
{
    public void Configure(EntityTypeBuilder<BillLineItem> builder)
    {
        builder.ToTable("BillLineItems", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Foreign key to Bill
        builder.Property(x => x.BillId)
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

        // Line total
        builder.Property(x => x.LineTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        // Optional account coding
        builder.Property(x => x.AccountId);

        // Indexes for query optimization
        
        // Index on BillId for foreign key lookups (one-to-many queries)
        builder.HasIndex(x => x.BillId)
            .HasDatabaseName("IX_BillLineItems_BillId");

        // Index on AccountId for expense analysis queries
        builder.HasIndex(x => x.AccountId)
            .HasDatabaseName("IX_BillLineItems_AccountId");

        // Composite index for reporting by bill and account
        builder.HasIndex(x => new { x.BillId, x.AccountId })
            .HasDatabaseName("IX_BillLineItems_Bill_Account");

        // Relationship to Bill (configured from BillConfiguration as well)
        // This ensures the foreign key relationship is properly established
        builder.HasOne<Bill>()
            .WithMany(b => b.LineItems)
            .HasForeignKey(x => x.BillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

