namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollectionStrategy entity.
/// </summary>
internal sealed class CollectionStrategyConfiguration : IEntityTypeConfiguration<CollectionStrategy>
{
    public void Configure(EntityTypeBuilder<CollectionStrategy> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(128);

        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.MinOutstandingAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxOutstandingAmount)
            .HasPrecision(18, 2);

    }
}