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

        builder.Property(x => x.TransactionType)
            .HasMaxLength(32);


        // Relationships
        builder.HasOne(x => x.ShareAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.ShareAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ShareAccountId)
            .HasDatabaseName("IX_ShareTransactions_ShareAccountId");

        builder.HasIndex(x => x.TransactionType)
            .HasDatabaseName("IX_ShareTransactions_TransactionType");
    }
}

