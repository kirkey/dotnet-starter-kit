using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(p => p.OrderNumber).HasMaxLength(64);
        builder.HasIndex(p => p.OrderNumber).IsUnique();

        builder.HasOne(po => po.Supplier)
               .WithMany(s => s.PurchaseOrders)
               .HasForeignKey(po => po.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(po => po.Warehouse)
               .WithMany(w => w.PurchaseOrders)
               .HasForeignKey(po => po.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(po => po.PurchaseOrderDetails)
               .WithOne(pod => pod.PurchaseOrder)
               .HasForeignKey(pod => pod.PurchaseOrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

