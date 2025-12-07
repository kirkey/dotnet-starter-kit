namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanDisbursementTranche entity.
/// </summary>
internal sealed class LoanDisbursementTrancheConfiguration : IEntityTypeConfiguration<LoanDisbursementTranche>
{
    public void Configure(EntityTypeBuilder<LoanDisbursementTranche> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrancheNumber)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.TrancheNumber);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Deductions)
            .HasPrecision(18, 2);

        builder.Property(x => x.NetAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.DisbursementMethod)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.DisbursementMethod);

        builder.Property(x => x.BankAccountNumber)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.BankAccountNumber);

        builder.Property(x => x.BankName)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.BankName);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.ReferenceNumber);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanDisbursementTranche.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.ApprovedByUserId);
        builder.HasIndex(x => x.DisbursedByUserId);
        builder.HasIndex(x => x.Status);
    }
}