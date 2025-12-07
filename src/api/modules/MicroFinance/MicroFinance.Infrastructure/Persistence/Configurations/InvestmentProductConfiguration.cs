namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InvestmentProduct entity.
/// </summary>
internal sealed class InvestmentProductConfiguration : IEntityTypeConfiguration<InvestmentProduct>
{
    public void Configure(EntityTypeBuilder<InvestmentProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.MinimumInvestment)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaximumInvestment)
            .HasPrecision(18, 2);

        builder.Property(x => x.ManagementFeePercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.PerformanceFeePercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.EntryLoadPercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExitLoadPercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExpectedReturnMin)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExpectedReturnMax)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentNav)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalAum)
            .HasPrecision(18, 2);

        builder.Property(x => x.YtdReturn)
            .HasPrecision(18, 2);

        builder.Property(x => x.OneYearReturn)
            .HasPrecision(18, 2);

        builder.Property(x => x.ThreeYearReturn)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}