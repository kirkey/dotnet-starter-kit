namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollectionCase entity.
/// </summary>
internal sealed class CollectionCaseConfiguration : IEntityTypeConfiguration<CollectionCase>
{
    public void Configure(EntityTypeBuilder<CollectionCase> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.AmountOverdue)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalOutstanding)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountRecovered)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}