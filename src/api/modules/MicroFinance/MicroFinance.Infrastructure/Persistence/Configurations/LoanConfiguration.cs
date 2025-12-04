namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Loan entity.
/// </summary>
internal sealed class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LoanNumber)
            .IsRequired()
            .HasMaxLength(LoanConstants.LoanNumberMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(LoanConstants.StatusMaxLength);

        builder.Property(x => x.RepaymentFrequency)
            .IsRequired()
            .HasMaxLength(LoanConstants.RepaymentFrequencyMaxLength);

        builder.Property(x => x.Purpose)
            .HasMaxLength(LoanConstants.PurposeMaxLength);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(LoanConstants.RejectionReasonMaxLength);

        builder.Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestRate).HasPrecision(5, 2);
        builder.Property(x => x.OutstandingPrincipal).HasPrecision(18, 2);
        builder.Property(x => x.OutstandingInterest).HasPrecision(18, 2);
        builder.Property(x => x.TotalPaid).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LoanProduct)
            .WithMany(p => p.Loans)
            .HasForeignKey(x => x.LoanProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.LoanNumber)
            .IsUnique()
            .HasDatabaseName("IX_Loans_LoanNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_Loans_MemberId");

        builder.HasIndex(x => x.LoanProductId)
            .HasDatabaseName("IX_Loans_LoanProductId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Loans_Status");

        builder.HasIndex(x => x.ApplicationDate)
            .HasDatabaseName("IX_Loans_ApplicationDate");

        builder.HasIndex(x => x.DisbursementDate)
            .HasDatabaseName("IX_Loans_DisbursementDate");
    }
}

