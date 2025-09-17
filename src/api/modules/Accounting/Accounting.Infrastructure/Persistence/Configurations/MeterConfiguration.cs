namespace Accounting.Infrastructure.Persistence.Configurations;

public class MeterConfiguration : IEntityTypeConfiguration<Meter>
{
    public void Configure(EntityTypeBuilder<Meter> builder)
    {
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}
