using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class StoreTransferConfiguration : IEntityTypeConfiguration<StoreTransfer>
{
    public void Configure(EntityTypeBuilder<StoreTransfer> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(st => st.TransferNumber).HasMaxLength(64);
        builder.HasIndex(st => st.TransferNumber).IsUnique();

        builder.HasOne(st => st.FromWarehouse)
               .WithMany(w => w.TransfersSent)
               .HasForeignKey(st => st.FromWarehouseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(st => st.ToStore)
               .WithMany(s => s.TransfersReceived)
               .HasForeignKey(st => st.ToStoreId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(st => st.StoreTransferDetails)
               .WithOne(std => std.StoreTransfer)
               .HasForeignKey(std => std.StoreTransferId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

