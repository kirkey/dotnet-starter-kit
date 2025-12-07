namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the AgentBanking entity.
/// </summary>
internal sealed class AgentBankingConfiguration : IEntityTypeConfiguration<AgentBanking>
{
    public void Configure(EntityTypeBuilder<AgentBanking> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.FloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MinFloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxFloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.CommissionRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCommissionEarned)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyTransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyTransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyVolumeProcessed)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyVolumeProcessed)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.LinkedStaffId);
        builder.HasIndex(x => x.Status);
    }
}