using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
{
    public void Configure(EntityTypeBuilder<SaleDetail> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasOne(sd => sd.Product)
               .WithMany()
               .HasForeignKey(sd => sd.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sd => sd.ProductBatch)
               .WithMany(pb => pb.SaleDetails)
               .HasForeignKey(sd => sd.ProductBatchId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}

