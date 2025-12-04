namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FixedDeposit entity.
/// </summary>
internal sealed class FixedDepositConfiguration : IEntityTypeConfiguration<FixedDeposit>
{
    public void Configure(EntityTypeBuilder<FixedDeposit> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CertificateNumber)
            .IsRequired()
            .HasMaxLength(FixedDeposit.CertificateNumberMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(FixedDeposit.StatusMaxLength);

        builder.Property(x => x.MaturityInstruction)
            .IsRequired()
            .HasMaxLength(FixedDeposit.MaturityInstructionMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(FixedDeposit.NotesMaxLength);

        builder.Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestRate).HasPrecision(5, 2);
        builder.Property(x => x.InterestEarned).HasPrecision(18, 2);
        builder.Property(x => x.InterestPaid).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SavingsProduct)
            .WithMany()
            .HasForeignKey(x => x.SavingsProductId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.LinkedSavingsAccount)
            .WithMany()
            .HasForeignKey(x => x.LinkedSavingsAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.CertificateNumber)
            .IsUnique()
            .HasDatabaseName("IX_FixedDeposits_CertificateNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_FixedDeposits_MemberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FixedDeposits_Status");

        builder.HasIndex(x => x.MaturityDate)
            .HasDatabaseName("IX_FixedDeposits_MaturityDate");

        builder.HasIndex(x => x.DepositDate)
            .HasDatabaseName("IX_FixedDeposits_DepositDate");
    }
}
