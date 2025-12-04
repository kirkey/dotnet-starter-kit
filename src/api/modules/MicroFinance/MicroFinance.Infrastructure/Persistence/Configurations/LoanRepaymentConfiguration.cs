namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanRepayment entity.
/// </summary>
internal sealed class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepayment>
{
    public void Configure(EntityTypeBuilder<LoanRepayment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(LoanRepayment.ReceiptNumberMaxLength);

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasMaxLength(LoanRepayment.PaymentMethodMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanRepayment.NotesMaxLength);

        builder.Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestAmount).HasPrecision(18, 2);
        builder.Property(x => x.PenaltyAmount).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Loan)
            .WithMany(l => l.LoanRepayments)
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ReceiptNumber)
            .IsUnique()
            .HasDatabaseName("IX_LoanRepayments_ReceiptNumber");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_LoanRepayments_LoanId");

        builder.HasIndex(x => x.RepaymentDate)
            .HasDatabaseName("IX_LoanRepayments_RepaymentDate");
    }
}

