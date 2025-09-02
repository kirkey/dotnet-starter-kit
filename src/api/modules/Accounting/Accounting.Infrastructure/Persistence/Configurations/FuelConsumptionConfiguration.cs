using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class FuelConsumptionConfiguration : IEntityTypeConfiguration<FuelConsumption>
{
    public void Configure(EntityTypeBuilder<FuelConsumption> builder)
    {
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}

