using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class WarehouseConfiguration : IEntityTypeConfiguration<FSH.Starter.WebApi.Warehouse.Domain.Warehouse>
{
    public void Configure(EntityTypeBuilder<FSH.Starter.WebApi.Warehouse.Domain.Warehouse> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Name).HasMaxLength(1024);
        builder.Property(p => p.Code).HasMaxLength(128);
        builder.HasIndex(p => p.Code).IsUnique();
    }
}

