namespace Accounting.Infrastructure.Persistence.Configurations;

public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
{
    public void Configure(EntityTypeBuilder<GeneralLedger> builder)
    {
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}

