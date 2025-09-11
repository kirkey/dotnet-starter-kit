using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class ProductMovementSummaryConfiguration : IEntityTypeConfiguration<ProductMovementSummary>
{
    public void Configure(EntityTypeBuilder<ProductMovementSummary> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.HasIndex(p => new { p.ProductId, p.PeriodStart, p.PeriodEnd });
    }
}

