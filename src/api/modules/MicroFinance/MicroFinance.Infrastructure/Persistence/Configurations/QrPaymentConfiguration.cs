namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the QrPayment entity.
/// </summary>
internal sealed class QrPaymentConfiguration : IEntityTypeConfiguration<QrPayment>
{
    public void Configure(EntityTypeBuilder<QrPayment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.WalletId);
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.AgentId);
        builder.HasIndex(x => x.LastTransactionId);
        builder.HasIndex(x => x.Status);
    }
}