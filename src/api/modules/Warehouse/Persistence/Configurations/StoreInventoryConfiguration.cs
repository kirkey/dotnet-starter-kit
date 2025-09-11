using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class StoreInventoryConfiguration : IEntityTypeConfiguration<StoreInventory>
{
    public void Configure(EntityTypeBuilder<StoreInventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasIndex(si => new { si.StoreId, si.ProductId }).IsUnique();

        builder.HasOne(si => si.Store)
               .WithMany()
               .HasForeignKey(si => si.StoreId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(si => si.Product)
               .WithMany(p => p.StoreInventories)
               .HasForeignKey(si => si.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

