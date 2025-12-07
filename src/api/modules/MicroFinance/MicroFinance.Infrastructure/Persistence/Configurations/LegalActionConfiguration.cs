namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LegalAction entity.
/// </summary>
internal sealed class LegalActionConfiguration : IEntityTypeConfiguration<LegalAction>
{
    public void Configure(EntityTypeBuilder<LegalAction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.ClaimAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.JudgmentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountRecovered)
            .HasPrecision(18, 2);

        builder.Property(x => x.LegalCosts)
            .HasPrecision(18, 2);

        builder.Property(x => x.CourtFees)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}