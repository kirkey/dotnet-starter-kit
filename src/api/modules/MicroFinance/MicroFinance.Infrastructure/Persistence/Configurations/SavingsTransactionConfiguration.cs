namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the SavingsTransaction entity.
/// </summary>
internal sealed class SavingsTransactionConfiguration : IEntityTypeConfiguration<SavingsTransaction>
{
    public void Configure(EntityTypeBuilder<SavingsTransaction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .IsRequired()
            .HasMaxLength(SavingsTransaction.ReferenceMaxLength);

        builder.Property(x => x.TransactionType)
            .IsRequired()
            .HasMaxLength(SavingsTransaction.TransactionTypeMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(SavingsTransaction.DescriptionMaxLength);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(SavingsTransaction.PaymentMethodMaxLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.BalanceAfter).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.SavingsAccount)
            .WithMany(a => a.Transactions)
            .HasForeignKey(x => x.SavingsAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Reference)
            .IsUnique()
            .HasDatabaseName("IX_SavingsTransactions_Reference");

        builder.HasIndex(x => x.SavingsAccountId)
            .HasDatabaseName("IX_SavingsTransactions_SavingsAccountId");

        builder.HasIndex(x => x.TransactionType)
            .HasDatabaseName("IX_SavingsTransactions_TransactionType");

        builder.HasIndex(x => x.TransactionDate)
            .HasDatabaseName("IX_SavingsTransactions_TransactionDate");
    }
}

