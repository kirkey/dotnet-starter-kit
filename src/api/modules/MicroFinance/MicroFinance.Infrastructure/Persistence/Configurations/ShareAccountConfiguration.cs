namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ShareAccount entity.
/// </summary>
internal sealed class ShareAccountConfiguration : IEntityTypeConfiguration<ShareAccount>
{
    public void Configure(EntityTypeBuilder<ShareAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountNumber)
            .IsRequired()
            .HasMaxLength(ShareAccount.AccountNumberMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(ShareAccount.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(ShareAccount.NotesMaxLength);

        builder.Property(x => x.TotalShareValue).HasPrecision(18, 2);
        builder.Property(x => x.TotalPurchases).HasPrecision(18, 2);
        builder.Property(x => x.TotalRedemptions).HasPrecision(18, 2);
        builder.Property(x => x.TotalDividendsEarned).HasPrecision(18, 2);
        builder.Property(x => x.TotalDividendsPaid).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShareProduct)
            .WithMany(p => p.ShareAccounts)
            .HasForeignKey(x => x.ShareProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.AccountNumber)
            .IsUnique()
            .HasDatabaseName("IX_ShareAccounts_AccountNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_ShareAccounts_MemberId");

        builder.HasIndex(x => x.ShareProductId)
            .HasDatabaseName("IX_ShareAccounts_ShareProductId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_ShareAccounts_Status");

        builder.HasIndex(x => x.OpenedDate)
            .HasDatabaseName("IX_ShareAccounts_OpenedDate");
    }
}
