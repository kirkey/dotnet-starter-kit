namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollateralValuation entity.
/// </summary>
internal sealed class CollateralValuationConfiguration : IEntityTypeConfiguration<CollateralValuation>
{
    public void Configure(EntityTypeBuilder<CollateralValuation> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.MarketValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.ForcedSaleValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.InsurableValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.PreviousValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.ValueChange)
            .HasPrecision(18, 2);

        builder.Property(x => x.ValueChangePercent)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.CollateralId);
        builder.HasIndex(x => x.ApprovedById);
        builder.HasIndex(x => x.Status);
    }
}