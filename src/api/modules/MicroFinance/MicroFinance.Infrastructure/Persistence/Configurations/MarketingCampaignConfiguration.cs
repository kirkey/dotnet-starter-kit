namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MarketingCampaign entity.
/// </summary>
internal sealed class MarketingCampaignConfiguration : IEntityTypeConfiguration<MarketingCampaign>
{
    public void Configure(EntityTypeBuilder<MarketingCampaign> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.Code)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Budget)
            .HasPrecision(18, 2);

        builder.Property(x => x.SpentAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ResponseRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.ConversionRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.Roi)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.CreatedById);
        builder.HasIndex(x => x.ApprovedById);
        builder.HasIndex(x => x.Status);
    }
}