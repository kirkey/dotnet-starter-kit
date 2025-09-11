using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasIndex(it => it.TransactionDate);

        builder.HasOne(it => it.Product)
               .WithMany()
               .HasForeignKey(it => it.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.ProductBatch)
               .WithMany(pb => pb.InventoryTransactions)
               .HasForeignKey(it => it.ProductBatchId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(it => it.Warehouse)
               .WithMany()
               .HasForeignKey(it => it.WarehouseId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(it => it.Store)
               .WithMany()
               .HasForeignKey(it => it.StoreId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}

