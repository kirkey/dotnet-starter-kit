namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InsuranceProduct entity.
/// </summary>
internal sealed class InsuranceProductConfiguration : IEntityTypeConfiguration<InsuranceProduct>
{
    public void Configure(EntityTypeBuilder<InsuranceProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(InsuranceProduct.MaxLengths.Code);

        builder.Property(x => x.InsuranceType)
            .HasMaxLength(InsuranceProduct.MaxLengths.InsuranceType);

        builder.Property(x => x.Provider)
            .HasMaxLength(InsuranceProduct.MaxLengths.Provider);

        builder.Property(x => x.MinCoverage)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxCoverage)
            .HasPrecision(18, 2);

        builder.Property(x => x.PremiumRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.TermsConditions)
            .HasMaxLength(InsuranceProduct.MaxLengths.TermsConditions);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.HasIndex(x => x.Status);
    }
}