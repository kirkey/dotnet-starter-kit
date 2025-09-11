using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class ExpiryAlertConfiguration : IEntityTypeConfiguration<ExpiryAlert>
{
    public void Configure(EntityTypeBuilder<ExpiryAlert> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.HasIndex(e => e.ExpiryDate);

        builder.HasOne(e => e.ProductBatch)
               .WithMany()
               .HasForeignKey(e => e.ProductBatchId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

