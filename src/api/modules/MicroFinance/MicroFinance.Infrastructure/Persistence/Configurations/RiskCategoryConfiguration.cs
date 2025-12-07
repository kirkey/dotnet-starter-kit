namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the RiskCategory entity.
/// </summary>
internal sealed class RiskCategoryConfiguration : IEntityTypeConfiguration<RiskCategory>
{
    public void Configure(EntityTypeBuilder<RiskCategory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(RiskCategory.MaxLengths.Code);

        builder.Property(x => x.Name)
            .HasMaxLength(RiskCategory.MaxLengths.Name);

        builder.Property(x => x.Description)
            .HasMaxLength(RiskCategory.MaxLengths.Description);

        builder.Property(x => x.RiskType)
            .HasMaxLength(RiskCategory.MaxLengths.RiskType);

        builder.Property(x => x.WeightFactor)
            .HasPrecision(18, 2);

        builder.Property(x => x.AlertThreshold)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(RiskCategory.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ParentCategoryId);
        builder.HasIndex(x => x.Status);
    }
}
