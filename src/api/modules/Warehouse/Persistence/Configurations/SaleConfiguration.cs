using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(s => s.SaleNumber).HasMaxLength(64);
        builder.HasIndex(s => s.SaleNumber).IsUnique();

        builder.HasOne(s => s.Store)
               .WithMany(st => st.Sales)
               .HasForeignKey(s => s.StoreId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Customer)
               .WithMany(c => c.Sales)
               .HasForeignKey(s => s.CustomerId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.SaleDetails)
               .WithOne(sd => sd.Sale)
               .HasForeignKey(sd => sd.SaleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Payments)
               .WithOne(p => p.Sale)
               .HasForeignKey(p => p.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

