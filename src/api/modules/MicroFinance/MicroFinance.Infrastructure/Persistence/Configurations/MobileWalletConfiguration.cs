namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MobileWallet entity.
/// </summary>
internal sealed class MobileWalletConfiguration : IEntityTypeConfiguration<MobileWallet>
{
    public void Configure(EntityTypeBuilder<MobileWallet> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Balance)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyUsed)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyUsed)
            .HasPrecision(18, 2);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Provider)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.ExternalWalletId)
            .HasMaxLength(256);

        builder.Property(x => x.Tier)
            .HasMaxLength(32);

        builder.Property(x => x.PinHash)
            .HasMaxLength(512);

        // Relationships
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SavingsAccount>()
            .WithMany()
            .HasForeignKey(x => x.LinkedSavingsAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("IX_MobileWallets_PhoneNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_MobileWallets_MemberId");

        builder.HasIndex(x => x.LinkedSavingsAccountId)
            .HasDatabaseName("IX_MobileWallets_LinkedSavingsAccountId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_MobileWallets_Status");
    }
}
