using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SalesImportItem.
/// </summary>
public class SalesImportItemConfiguration : IEntityTypeConfiguration<SalesImportItem>
{
    public void Configure(EntityTypeBuilder<SalesImportItem> builder)
    {
        builder.ToTable("SalesImportItems", SchemaNames.Store);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SalesImportId)
            .IsRequired();

        builder.Property(x => x.LineNumber)
            .IsRequired();

        builder.Property(x => x.SaleDate)
            .IsRequired();

        builder.Property(x => x.Barcode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ItemName)
            .HasMaxLength(255);

        builder.Property(x => x.QuantitySold)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(16, 2);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(16, 2);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(x => x.SalesImport)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.SalesImportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.InventoryTransaction)
            .WithMany()
            .HasForeignKey(x => x.InventoryTransactionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Indexes for performance
        builder.HasIndex(x => x.SalesImportId);
        builder.HasIndex(x => x.Barcode);
        builder.HasIndex(x => x.ItemId);
        builder.HasIndex(x => new { x.SalesImportId, x.LineNumber })
            .IsUnique();
    }
}

