using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
{
    public void Configure(EntityTypeBuilder<ProductBatch> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(p => p.BatchNumber).HasMaxLength(128);
        builder.Property(p => p.LotNumber).HasMaxLength(128);

        builder.HasOne(pb => pb.Product)
               .WithMany(p => p.ProductBatches)
               .HasForeignKey(pb => pb.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pb => pb.PurchaseOrderDetail)
               .WithMany(pod => pod.ProductBatches)
               .HasForeignKey(pb => pb.PurchaseOrderDetailId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}

