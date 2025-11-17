using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configuration;

/// <summary>
/// Entity Type Configuration for TaxMaster.
/// Configures database mapping, indexes, and constraints for tax master entities.
/// </summary>
public sealed class TaxMasterConfiguration : IEntityTypeConfiguration<TaxMaster>
{
    /// <summary>
    /// Configures the TaxMaster entity mapping.
    /// </summary>
    /// <param name="builder">Entity type builder.</param>
    public void Configure(EntityTypeBuilder<TaxMaster> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.TaxType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Rate)
            .IsRequired()
            .HasPrecision(5, 4);

        builder.Property(x => x.IsCompound)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.Jurisdiction)
            .HasMaxLength(100);

        builder.Property(x => x.EffectiveDate)
            .IsRequired();

        builder.Property(x => x.ExpiryDate);

        builder.Property(x => x.TaxCollectedAccountId)
            .IsRequired();

        builder.Property(x => x.TaxPaidAccountId);

        builder.Property(x => x.TaxAuthority)
            .HasMaxLength(200);

        builder.Property(x => x.TaxRegistrationNumber)
            .HasMaxLength(100);

        builder.Property(x => x.ReportingCategory)
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Add indexes for common queries
        builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("idx_tax_master_code");
        builder.HasIndex(x => x.TaxType).HasDatabaseName("idx_tax_master_tax_type");
        builder.HasIndex(x => x.IsActive).HasDatabaseName("idx_tax_master_is_active");
        builder.HasIndex(x => x.Jurisdiction).HasDatabaseName("idx_tax_master_jurisdiction");
        builder.HasIndex(x => new { x.TaxType, x.Jurisdiction, x.EffectiveDate })
            .HasDatabaseName("idx_tax_master_type_jurisdiction_date");

        // Table configuration
        builder.ToTable(nameof(TaxMaster), SchemaNames.HumanResources);
    }
}

