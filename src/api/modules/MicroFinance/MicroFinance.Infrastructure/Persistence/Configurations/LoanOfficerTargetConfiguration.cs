namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanOfficerTarget entity.
/// </summary>
internal sealed class LoanOfficerTargetConfiguration : IEntityTypeConfiguration<LoanOfficerTarget>
{
    public void Configure(EntityTypeBuilder<LoanOfficerTarget> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TargetType)
            .HasMaxLength(LoanOfficerTarget.MaxLengths.TargetType);

        builder.Property(x => x.Description)
            .HasMaxLength(LoanOfficerTarget.MaxLengths.Description);

        builder.Property(x => x.Period)
            .HasMaxLength(LoanOfficerTarget.MaxLengths.Period);

        builder.Property(x => x.TargetValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.MetricUnit)
            .HasMaxLength(LoanOfficerTarget.MaxLengths.MetricUnit);

        builder.Property(x => x.AchievedValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.AchievementPercentage)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.MinimumThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.StretchTarget)
            .HasPrecision(18, 2);

        builder.Property(x => x.Weight)
            .HasPrecision(18, 2);

        builder.Property(x => x.IncentiveAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.StretchBonus)
            .HasPrecision(18, 2);


        // Relationships
        builder.HasOne(x => x.Staff)
            .WithMany(x => x.LoanOfficerTargets)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.StaffId);
        builder.HasIndex(x => x.Status);
    }
}
