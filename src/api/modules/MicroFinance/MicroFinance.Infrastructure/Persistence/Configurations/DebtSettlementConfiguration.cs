namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the DebtSettlement entity.
/// </summary>
internal sealed class DebtSettlementConfiguration : IEntityTypeConfiguration<DebtSettlement>
{
    public void Configure(EntityTypeBuilder<DebtSettlement> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.OriginalOutstanding)
            .HasPrecision(18, 2);

        builder.Property(x => x.SettlementAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountPercentage)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountPaid)
            .HasPrecision(18, 2);

        builder.Property(x => x.RemainingBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.InstallmentAmount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}