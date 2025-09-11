using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class StoreTransferDetailConfiguration : IEntityTypeConfiguration<StoreTransferDetail>
{
    public void Configure(EntityTypeBuilder<StoreTransferDetail> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasOne(std => std.Product)
               .WithMany()
               .HasForeignKey(std => std.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(std => std.ProductBatch)
               .WithMany()
               .HasForeignKey(std => std.ProductBatchId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}

