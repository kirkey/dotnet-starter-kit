using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class WarehouseInventoryConfiguration : IEntityTypeConfiguration<WarehouseInventory>
{
    public void Configure(EntityTypeBuilder<WarehouseInventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasIndex(wi => new { wi.WarehouseId, wi.ProductId }).IsUnique();

        builder.HasOne(wi => wi.Warehouse)
               .WithMany()
               .HasForeignKey(wi => wi.WarehouseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wi => wi.Product)
               .WithMany(p => p.WarehouseInventories)
               .HasForeignKey(wi => wi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

