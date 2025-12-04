namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ShareTransaction entity.
/// </summary>
internal sealed class ShareTransactionConfiguration : IEntityTypeConfiguration<ShareTransaction>
{
    public void Configure(EntityTypeBuilder<ShareTransaction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .IsRequired()
            .HasMaxLength(ShareTransaction.ReferenceMaxLength);

        builder.Property(x => x.TransactionType)
            .IsRequired()
            .HasMaxLength(ShareTransaction.TransactionTypeMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(ShareTransaction.DescriptionMaxLength);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(ShareTransaction.PaymentMethodMaxLength);

        builder.Property(x => x.PricePerShare).HasPrecision(18, 2);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.ShareAccount)
            .WithMany(a => a.Transactions)
            .HasForeignKey(x => x.ShareAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Reference)
            .IsUnique()
            .HasDatabaseName("IX_ShareTransactions_Reference");

        builder.HasIndex(x => x.ShareAccountId)
            .HasDatabaseName("IX_ShareTransactions_ShareAccountId");

        builder.HasIndex(x => x.TransactionType)
            .HasDatabaseName("IX_ShareTransactions_TransactionType");

        builder.HasIndex(x => x.TransactionDate)
            .HasDatabaseName("IX_ShareTransactions_TransactionDate");
    }
}
