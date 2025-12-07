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

        builder.Property(x => x.TransactionReference)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.TransactionType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.SourcePhone)
            .HasMaxLength(32);

        builder.Property(x => x.DestinationPhone)
            .HasMaxLength(32);

        builder.Property(x => x.ProviderReference)
            .HasMaxLength(256);

        builder.Property(x => x.ProviderResponse)
            .HasMaxLength(2048);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1024);

        // Relationships
        builder.HasOne<MobileWallet>()
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MobileWallet>()
            .WithMany()
            .HasForeignKey(x => x.RecipientWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Loan>()
            .WithMany()
            .HasForeignKey(x => x.LinkedLoanId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<SavingsAccount>()
            .WithMany()
            .HasForeignKey(x => x.LinkedSavingsAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<MobileTransaction>()
            .WithMany()
            .HasForeignKey(x => x.ReversalOfTransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.TransactionReference)
            .IsUnique()
            .HasDatabaseName("IX_MobileTransactions_TransactionReference");

        builder.HasIndex(x => x.WalletId)
            .HasDatabaseName("IX_MobileTransactions_WalletId");

        builder.HasIndex(x => x.RecipientWalletId)
            .HasDatabaseName("IX_MobileTransactions_RecipientWalletId");

        builder.HasIndex(x => x.LinkedLoanId)
            .HasDatabaseName("IX_MobileTransactions_LinkedLoanId");

        builder.HasIndex(x => x.LinkedSavingsAccountId)
            .HasDatabaseName("IX_MobileTransactions_LinkedSavingsAccountId");

        builder.HasIndex(x => x.ReversalOfTransactionId)
            .HasDatabaseName("IX_MobileTransactions_ReversalOfTransactionId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_MobileTransactions_Status");

        builder.HasIndex(x => x.InitiatedAt)
            .HasDatabaseName("IX_MobileTransactions_InitiatedAt");
    }
}
