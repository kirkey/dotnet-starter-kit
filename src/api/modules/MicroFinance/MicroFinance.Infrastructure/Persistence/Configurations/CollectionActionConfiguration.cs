namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollectionAction entity.
/// </summary>
internal sealed class CollectionActionConfiguration : IEntityTypeConfiguration<CollectionAction>
{
    public void Configure(EntityTypeBuilder<CollectionAction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromisedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Latitude)
            .HasPrecision(18, 2);

        builder.Property(x => x.Longitude)
            .HasPrecision(18, 2);

    }
}