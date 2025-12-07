namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MobileWallet entity.
/// </summary>
internal sealed class MobileWalletConfiguration : IEntityTypeConfiguration<MobileWallet>
{
    public void Configure(EntityTypeBuilder<MobileWallet> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Balance)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyUsed)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyUsed)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LinkedSavingsAccountId);
        builder.HasIndex(x => x.Status);
    }
}