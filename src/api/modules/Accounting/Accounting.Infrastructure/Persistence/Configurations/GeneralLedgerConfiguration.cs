using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
{
    public void Configure(EntityTypeBuilder<GeneralLedger> builder)
    {
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}

