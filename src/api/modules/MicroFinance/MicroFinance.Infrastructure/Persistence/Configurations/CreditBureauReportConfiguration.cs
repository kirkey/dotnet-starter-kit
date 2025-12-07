namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CreditBureauReport entity.
/// </summary>
internal sealed class CreditBureauReportConfiguration : IEntityTypeConfiguration<CreditBureauReport>
{
    public void Configure(EntityTypeBuilder<CreditBureauReport> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReportNumber)
            .HasMaxLength(CreditBureauReport.MaxLengths.ReportNumber);

        builder.Property(x => x.BureauName)
            .HasMaxLength(CreditBureauReport.MaxLengths.BureauName);

        builder.Property(x => x.ScoreModel)
            .HasMaxLength(CreditBureauReport.MaxLengths.ScoreModel);

        builder.Property(x => x.RiskGrade)
            .HasMaxLength(CreditBureauReport.MaxLengths.RiskGrade);

        builder.Property(x => x.TotalOutstandingBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCreditLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.CreditUtilization)
            .HasPrecision(18, 2);

        builder.Property(x => x.DebtToIncomeRatio)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(CreditBureauReport.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Note: One-to-one relationship with CreditBureauInquiry is configured from CreditBureauInquiry side

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.Status);
    }
}
