namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollateralInsurance entity.
/// </summary>
internal sealed class CollateralInsuranceConfiguration : IEntityTypeConfiguration<CollateralInsurance>
{
    public void Configure(EntityTypeBuilder<CollateralInsurance> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.CoverageAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.PremiumAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Deductible)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.CollateralId);
        builder.HasIndex(x => x.Status);
    }
}