using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class ConsumptionDataConfiguration : IEntityTypeConfiguration<ConsumptionData>
{
    public void Configure(EntityTypeBuilder<ConsumptionData> builder)
    {
        // Define property mappings, keys, relationships, etc.
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}
