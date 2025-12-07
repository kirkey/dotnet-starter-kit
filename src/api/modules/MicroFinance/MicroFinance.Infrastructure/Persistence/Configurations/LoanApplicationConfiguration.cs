namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanApplication entity.
/// </summary>
internal sealed class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
{
    public void Configure(EntityTypeBuilder<LoanApplication> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApplicationNumber)
            .HasMaxLength(LoanApplication.MaxLengths.ApplicationNumber);

        builder.Property(x => x.RequestedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ApprovedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Purpose)
            .HasMaxLength(LoanApplication.MaxLengths.Purpose);

        builder.Property(x => x.BusinessType)
            .HasMaxLength(LoanApplication.MaxLengths.BusinessType);

        builder.Property(x => x.MonthlyIncome)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyExpenses)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExistingDebt)
            .HasPrecision(18, 2);

        builder.Property(x => x.DebtToIncomeRatio)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(LoanApplication.MaxLengths.RejectionReason);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanApplication.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LoanProduct)
            .WithMany()
            .HasForeignKey(x => x.LoanProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MemberGroup)
            .WithMany()
            .HasForeignKey(x => x.MemberGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LoanProductId);
        builder.HasIndex(x => x.MemberGroupId);
        builder.HasIndex(x => x.AssignedOfficerId);
        builder.HasIndex(x => x.DecisionByUserId);
        builder.HasIndex(x => x.Status);
    }
}