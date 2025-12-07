namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CreditScore entity.
/// </summary>
internal sealed class CreditScoreConfiguration : IEntityTypeConfiguration<CreditScore>
{
    public void Configure(EntityTypeBuilder<CreditScore> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ScoreType)
            .HasMaxLength(CreditScore.MaxLengths.ScoreType);

        builder.Property(x => x.ScoreModel)
            .HasMaxLength(CreditScore.MaxLengths.ScoreModel);

        builder.Property(x => x.Score)
            .HasPrecision(18, 2);

        builder.Property(x => x.ScoreMin)
            .HasPrecision(18, 2);

        builder.Property(x => x.ScoreMax)
            .HasPrecision(18, 2);

        builder.Property(x => x.ScorePercentile)
            .HasPrecision(18, 2);

        builder.Property(x => x.Grade)
            .HasMaxLength(CreditScore.MaxLengths.Grade);

        builder.Property(x => x.ProbabilityOfDefault)
            .HasPrecision(18, 2);

        builder.Property(x => x.LossGivenDefault)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExposureAtDefault)
            .HasPrecision(18, 2);

        builder.Property(x => x.ExpectedLoss)
            .HasPrecision(18, 2);

        builder.Property(x => x.Source)
            .HasMaxLength(CreditScore.MaxLengths.Source);

        builder.Property(x => x.ScoreChange)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(CreditScore.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreditBureauReport)
            .WithMany()
            .HasForeignKey(x => x.CreditBureauReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PreviousScore)
            .WithMany()
            .HasForeignKey(x => x.PreviousScoreId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.CreditBureauReportId);
        builder.HasIndex(x => x.PreviousScoreId);
        builder.HasIndex(x => x.Status);
    }
}