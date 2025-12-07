namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollateralRelease entity.
/// </summary>
internal sealed class CollateralReleaseConfiguration : IEntityTypeConfiguration<CollateralRelease>
{
    public void Configure(EntityTypeBuilder<CollateralRelease> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Indexes
        builder.HasIndex(x => x.CollateralId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.RequestedById);
        builder.HasIndex(x => x.ApprovedById);
        builder.HasIndex(x => x.ReleasedById);
        builder.HasIndex(x => x.Status);
    }
}