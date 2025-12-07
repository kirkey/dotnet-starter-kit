namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the AmlAlert entity.
/// </summary>
internal sealed class AmlAlertConfiguration : IEntityTypeConfiguration<AmlAlert>
{
    public void Configure(EntityTypeBuilder<AmlAlert> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.TransactionAmount)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Status);
    }
}