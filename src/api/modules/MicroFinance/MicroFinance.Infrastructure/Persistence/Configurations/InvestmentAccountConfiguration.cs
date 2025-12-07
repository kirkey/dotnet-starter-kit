namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InvestmentAccount entity.
/// </summary>
internal sealed class InvestmentAccountConfiguration : IEntityTypeConfiguration<InvestmentAccount>
{
    public void Configure(EntityTypeBuilder<InvestmentAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.TotalInvested)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalGainLoss)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalGainLossPercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.RealizedGains)
            .HasPrecision(18, 2);

        builder.Property(x => x.UnrealizedGains)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalDividends)
            .HasPrecision(18, 2);

        builder.Property(x => x.SipAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TargetAmount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.AssignedAdvisorId);
        builder.HasIndex(x => x.LinkedSavingsAccountId);
        builder.HasIndex(x => x.Status);
    }
}