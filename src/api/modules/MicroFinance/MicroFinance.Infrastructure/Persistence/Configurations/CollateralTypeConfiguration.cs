namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollateralType entity.
/// </summary>
internal sealed class CollateralTypeConfiguration : IEntityTypeConfiguration<CollateralType>
{
    public void Configure(EntityTypeBuilder<CollateralType> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.Code)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.DefaultLtvPercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxLtvPercent)
            .HasPrecision(18, 2);

        builder.Property(x => x.AnnualDepreciationRate)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}