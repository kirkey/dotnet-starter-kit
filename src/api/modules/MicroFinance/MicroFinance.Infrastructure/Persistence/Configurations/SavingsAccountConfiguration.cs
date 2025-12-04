namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the SavingsAccount entity.
/// </summary>
internal sealed class SavingsAccountConfiguration : IEntityTypeConfiguration<SavingsAccount>
{
    public void Configure(EntityTypeBuilder<SavingsAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountNumber)
            .IsRequired()
            .HasMaxLength(SavingsAccount.AccountNumberMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(SavingsAccount.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(SavingsAccount.NotesMaxLength);

        builder.Property(x => x.Balance).HasPrecision(18, 2);
        builder.Property(x => x.TotalDeposits).HasPrecision(18, 2);
        builder.Property(x => x.TotalWithdrawals).HasPrecision(18, 2);
        builder.Property(x => x.TotalInterestEarned).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany(m => m.SavingsAccounts)
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SavingsProduct)
            .WithMany(p => p.SavingsAccounts)
            .HasForeignKey(x => x.SavingsProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.AccountNumber)
            .IsUnique()
            .HasDatabaseName("IX_SavingsAccounts_AccountNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_SavingsAccounts_MemberId");

        builder.HasIndex(x => x.SavingsProductId)
            .HasDatabaseName("IX_SavingsAccounts_SavingsProductId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_SavingsAccounts_Status");

        builder.HasIndex(x => x.OpenedDate)
            .HasDatabaseName("IX_SavingsAccounts_OpenedDate");
    }
}

