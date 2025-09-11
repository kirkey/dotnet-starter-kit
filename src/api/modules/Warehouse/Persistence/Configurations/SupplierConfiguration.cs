using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Name).HasMaxLength(256);
        builder.Property(p => p.Code).HasMaxLength(128);
        builder.HasIndex(p => p.Code).IsUnique();
        builder.Property(p => p.Email).HasMaxLength(256);
        builder.Property(p => p.Phone).HasMaxLength(64);

        builder.HasMany(s => s.PurchaseOrders)
               .WithOne(po => po.Supplier)
               .HasForeignKey(po => po.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.SupplierProducts)
               .WithOne(sp => sp.Supplier)
               .HasForeignKey(sp => sp.SupplierId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

