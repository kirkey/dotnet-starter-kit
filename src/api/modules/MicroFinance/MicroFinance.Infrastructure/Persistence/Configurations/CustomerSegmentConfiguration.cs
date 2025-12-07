namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CustomerSegment entity.
/// </summary>
internal sealed class CustomerSegmentConfiguration : IEntityTypeConfiguration<CustomerSegment>
{
    public void Configure(EntityTypeBuilder<CustomerSegment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.Code)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.MinIncomeLevel)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxIncomeLevel)
            .HasPrecision(18, 2);

        builder.Property(x => x.DefaultInterestModifier)
            .HasPrecision(18, 2);

        builder.Property(x => x.DefaultFeeModifier)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}