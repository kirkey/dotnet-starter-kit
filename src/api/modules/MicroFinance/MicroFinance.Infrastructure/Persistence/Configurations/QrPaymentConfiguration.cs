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

        builder.Property(x => x.QrCode)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.QrType)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Reference)
            .HasMaxLength(QrPayment.ReferenceMaxLength);


        // Relationships
        builder.HasOne<MobileWallet>()
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<AgentBanking>()
            .WithMany()
            .HasForeignKey(x => x.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<MobileTransaction>()
            .WithMany()
            .HasForeignKey(x => x.LastTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.QrCode)
            .IsUnique()
            .HasDatabaseName("IX_QrPayments_QrCode");

        builder.HasIndex(x => x.WalletId)
            .HasDatabaseName("IX_QrPayments_WalletId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_QrPayments_MemberId");

        builder.HasIndex(x => x.AgentId)
            .HasDatabaseName("IX_QrPayments_AgentId");

        builder.HasIndex(x => x.LastTransactionId)
            .HasDatabaseName("IX_QrPayments_LastTransactionId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_QrPayments_Status");
    }
}
