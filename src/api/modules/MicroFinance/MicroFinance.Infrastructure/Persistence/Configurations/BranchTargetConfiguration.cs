namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the BranchTarget entity.
/// </summary>
internal sealed class BranchTargetConfiguration : IEntityTypeConfiguration<BranchTarget>
{
    public void Configure(EntityTypeBuilder<BranchTarget> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TargetType)
            .HasMaxLength(BranchTarget.MaxLengths.TargetType);

        builder.Property(x => x.Description)
            .HasMaxLength(BranchTarget.MaxLengths.Description);

        builder.Property(x => x.Period)
            .HasMaxLength(BranchTarget.MaxLengths.Period);

        builder.Property(x => x.TargetValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.MetricUnit)
            .HasMaxLength(BranchTarget.MaxLengths.MetricUnit);

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

        builder.Property(x => x.Notes)
            .HasMaxLength(BranchTarget.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.Status);
    }
}