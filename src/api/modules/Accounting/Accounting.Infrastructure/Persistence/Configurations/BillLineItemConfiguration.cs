namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for BillLineItem entity.
/// Defines database schema, constraints, indexes, and relationships.
/// </summary>
public class BillLineItemConfiguration : IEntityTypeConfiguration<BillLineItem>
{
    public void Configure(EntityTypeBuilder<BillLineItem> builder)
    {
        builder.ToTable("BillLineItems", "accounting");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillId)
            .IsRequired();

        builder.Property(x => x.LineNumber)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.ChartOfAccountId)
            .IsRequired();

        builder.Property(x => x.TaxCodeId)
            .IsRequired(false);

        builder.Property(x => x.TaxAmount)
            .IsRequired()
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.ProjectId)
            .IsRequired(false);

        builder.Property(x => x.CostCenterId)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        // Indexes for performance
        builder.HasIndex(x => x.BillId)
            .HasDatabaseName("IX_BillLineItems_BillId");

        builder.HasIndex(x => x.ChartOfAccountId)
            .HasDatabaseName("IX_BillLineItems_ChartOfAccountId");

        builder.HasIndex(x => x.TaxCodeId)
            .HasDatabaseName("IX_BillLineItems_TaxCodeId");

        builder.HasIndex(x => x.ProjectId)
            .HasDatabaseName("IX_BillLineItems_ProjectId");

        builder.HasIndex(x => x.CostCenterId)
            .HasDatabaseName("IX_BillLineItems_CostCenterId");

        // Composite index for bill line ordering
        builder.HasIndex(x => new { x.BillId, x.LineNumber })
            .IsUnique()
            .HasDatabaseName("IX_BillLineItems_BillId_LineNumber");
    }
}

