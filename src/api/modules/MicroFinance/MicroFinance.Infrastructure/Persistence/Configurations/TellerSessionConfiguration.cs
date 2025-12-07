namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the TellerSession entity.
/// </summary>
internal sealed class TellerSessionConfiguration : IEntityTypeConfiguration<TellerSession>
{
    public void Configure(EntityTypeBuilder<TellerSession> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionNumber)
            .HasMaxLength(TellerSession.MaxLengths.SessionNumber);

        builder.Property(x => x.TellerName)
            .HasMaxLength(TellerSession.MaxLengths.TellerName);

        builder.Property(x => x.OpeningBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCashIn)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCashOut)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExpectedClosingBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.ActualClosingBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Variance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.SupervisorName)
            .HasMaxLength(TellerSession.MaxLengths.SupervisorName);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CashVault)
            .WithMany()
            .HasForeignKey(x => x.CashVaultId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.CashVaultId);
        builder.HasIndex(x => x.TellerUserId);
        builder.HasIndex(x => x.SupervisorUserId);
        builder.HasIndex(x => x.Status);
    }
}