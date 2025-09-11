using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class PurchaseOrderDetailConfiguration : IEntityTypeConfiguration<PurchaseOrderDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderDetail> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasOne(pod => pod.PurchaseOrder)
               .WithMany(po => po.PurchaseOrderDetails)
               .HasForeignKey(pod => pod.PurchaseOrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pod => pod.Product)
               .WithMany()
               .HasForeignKey(pod => pod.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

