using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class SupplierProductConfiguration : IEntityTypeConfiguration<SupplierProduct>
{
    public void Configure(EntityTypeBuilder<SupplierProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasIndex(sp => new { sp.SupplierId, sp.ProductId })
               .IsUnique();

        builder.HasOne(sp => sp.Supplier)
               .WithMany(s => s.SupplierProducts)
               .HasForeignKey(sp => sp.SupplierId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Product)
               .WithMany(p => p.SupplierProducts)
               .HasForeignKey(sp => sp.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

