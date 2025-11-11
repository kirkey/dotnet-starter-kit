using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SalesImport.
/// </summary>
public class SalesImportConfiguration : IEntityTypeConfiguration<SalesImport>
{
    public void Configure(EntityTypeBuilder<SalesImport> builder)
    {
        builder.ToTable("SalesImports", SchemaNames.Store);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImportNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.ImportNumber)
            .IsUnique();

        builder.Property(x => x.ImportDate)
            .IsRequired();

        builder.Property(x => x.SalesPeriodFrom)
            .IsRequired();

        builder.Property(x => x.SalesPeriodTo)
            .IsRequired();

        builder.Property(x => x.WarehouseId)
            .IsRequired();

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.ReversalReason)
            .HasMaxLength(500);

        builder.Property(x => x.ProcessedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ReversedBy)
            .HasMaxLength(256);

        // Relationships
        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.SalesImport)
            .HasForeignKey(x => x.SalesImportId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.ImportDate);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => new { x.SalesPeriodFrom, x.SalesPeriodTo });
    }
}

