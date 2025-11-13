using Shared.Constants;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Company.
/// </summary>
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies", SchemaNames.HumanResources);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.CompanyCode)
            .IsUnique()
            .HasDatabaseName("IX_Companies_CompanyCode");

        builder.Property(c => c.LegalName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.TradeName)
            .HasMaxLength(256);

        builder.Property(c => c.TaxId)
            .HasMaxLength(50);

        builder.Property(c => c.BaseCurrency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(c => c.FiscalYearEnd)
            .IsRequired();

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.State)
            .HasMaxLength(100);

        builder.Property(c => c.ZipCode)
            .HasMaxLength(20);

        builder.Property(c => c.Country)
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .HasMaxLength(256);

        builder.Property(c => c.Website)
            .HasMaxLength(500);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.ParentCompanyId);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Notes)
            .HasMaxLength(2000);

        // Indexes for performance
        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Companies_IsActive");

        builder.HasIndex(c => c.ParentCompanyId)
            .HasDatabaseName("IX_Companies_ParentCompanyId");

        // Audit fields
        builder.Property(c => c.CreatedBy)
            .HasMaxLength(256);

        builder.Property(c => c.LastModifiedBy)
            .HasMaxLength(256);
    }
}
