namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the RiskIndicator entity.
/// </summary>
internal sealed class RiskIndicatorConfiguration : IEntityTypeConfiguration<RiskIndicator>
{
    public void Configure(EntityTypeBuilder<RiskIndicator> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(RiskIndicator.MaxLengths.Code);

        builder.Property(x => x.Name)
            .HasMaxLength(RiskIndicator.MaxLengths.Name);

        builder.Property(x => x.Description)
            .HasMaxLength(RiskIndicator.MaxLengths.Description);

        builder.Property(x => x.Formula)
            .HasMaxLength(RiskIndicator.MaxLengths.Formula);

        builder.Property(x => x.Unit)
            .HasMaxLength(RiskIndicator.MaxLengths.Unit);

        builder.Property(x => x.DataSource)
            .HasMaxLength(RiskIndicator.MaxLengths.DataSource);

        builder.Property(x => x.TargetValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.GreenThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.YellowThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.OrangeThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.RedThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.CurrentValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.PreviousValue)
            .HasPrecision(18, 2);

        builder.Property(x => x.WeightFactor)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(RiskIndicator.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.RiskCategory)
            .WithMany(x => x.Indicators)
            .HasForeignKey(x => x.RiskCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.RiskCategoryId);
        builder.HasIndex(x => x.Status);
    }
}
