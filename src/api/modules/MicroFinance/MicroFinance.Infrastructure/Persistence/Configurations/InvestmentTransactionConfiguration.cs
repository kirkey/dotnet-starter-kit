namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the InvestmentTransaction entity.
/// </summary>
internal sealed class InvestmentTransactionConfiguration : IEntityTypeConfiguration<InvestmentTransaction>
{
    public void Configure(EntityTypeBuilder<InvestmentTransaction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Units)
            .HasPrecision(18, 2);

        builder.Property(x => x.NavAtTransaction)
            .HasPrecision(18, 2);

        builder.Property(x => x.EntryLoadAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExitLoadAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.NetAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.GainLoss)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.InvestmentAccountId);
        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.SwitchToProductId);
        builder.HasIndex(x => x.SourceAccountId);
        builder.HasIndex(x => x.Status);
    }
}