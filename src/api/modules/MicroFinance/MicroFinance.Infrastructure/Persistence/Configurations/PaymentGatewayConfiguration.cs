namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the PaymentGateway entity.
/// </summary>
internal sealed class PaymentGatewayConfiguration : IEntityTypeConfiguration<PaymentGateway>
{
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.TransactionFeePercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.TransactionFeeFixed)
            .HasPrecision(18, 2);

        builder.Property(x => x.MinTransactionAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxTransactionAmount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}