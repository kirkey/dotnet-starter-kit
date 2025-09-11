using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Name).HasMaxLength(512);
        builder.Property(p => p.SKU).HasMaxLength(128);
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.Barcode).HasMaxLength(256);
        builder.Property(p => p.Brand).HasMaxLength(256);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

