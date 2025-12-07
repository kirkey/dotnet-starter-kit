namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the PromiseToPay entity.
/// </summary>
internal sealed class PromiseToPayConfiguration : IEntityTypeConfiguration<PromiseToPay>
{
    public void Configure(EntityTypeBuilder<PromiseToPay> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromisedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.ActualAmountPaid)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.HasIndex(x => x.Status);
    }
}