namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MobileTransaction entity.
/// </summary>
internal sealed class MobileTransactionConfiguration : IEntityTypeConfiguration<MobileTransaction>
{
    public void Configure(EntityTypeBuilder<MobileTransaction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Fee)
            .HasPrecision(18, 2);

        builder.Property(x => x.NetAmount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.WalletId);
        builder.HasIndex(x => x.RecipientWalletId);
        builder.HasIndex(x => x.LinkedLoanId);
        builder.HasIndex(x => x.LinkedSavingsAccountId);
        builder.HasIndex(x => x.ReversalOfTransactionId);
        builder.HasIndex(x => x.Status);
    }
}