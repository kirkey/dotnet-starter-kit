namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CashVault entity.
/// </summary>
internal sealed class CashVaultConfiguration : IEntityTypeConfiguration<CashVault>
{
    public void Configure(EntityTypeBuilder<CashVault> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(CashVault.MaxLengths.Code);

        builder.Property(x => x.VaultType)
            .HasMaxLength(CashVault.MaxLengths.VaultType);

        builder.Property(x => x.CurrentBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.OpeningBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MinimumBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaximumBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Location)
            .HasMaxLength(CashVault.MaxLengths.Location);

        builder.Property(x => x.CustodianName)
            .HasMaxLength(CashVault.MaxLengths.CustodianName);

        builder.Property(x => x.LastReconciledBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany(x => x.CashVaults)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.CustodianUserId);
        builder.HasIndex(x => x.Status);
    }
}
