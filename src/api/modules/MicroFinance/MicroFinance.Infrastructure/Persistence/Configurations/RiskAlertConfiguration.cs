namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the RiskAlert entity.
/// </summary>
internal sealed class RiskAlertConfiguration : IEntityTypeConfiguration<RiskAlert>
{
    public void Configure(EntityTypeBuilder<RiskAlert> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AlertNumber)
            .HasMaxLength(RiskAlert.MaxLengths.AlertNumber);

        builder.Property(x => x.Title)
            .HasMaxLength(RiskAlert.MaxLengths.Title);

        builder.Property(x => x.Description)
            .HasMaxLength(RiskAlert.MaxLengths.Description);

        builder.Property(x => x.Source)
            .HasMaxLength(RiskAlert.MaxLengths.Source);

        builder.Property(x => x.ThresholdValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.ActualValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.Variance)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Resolution)
            .HasMaxLength(RiskAlert.MaxLengths.Resolution);

        builder.Property(x => x.Notes)
            .HasMaxLength(RiskAlert.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.RiskCategory)
            .WithMany()
            .HasForeignKey(x => x.RiskCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RiskIndicator)
            .WithMany()
            .HasForeignKey(x => x.RiskIndicatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.RiskCategoryId);
        builder.HasIndex(x => x.RiskIndicatorId);
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.Status);
    }
}