using Finbuckle.MultiTenant;
using FSH.Starter.WebApi.Warehouse.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Warehouse.Persistence.Configurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.HasOne(p => p.Sale)
               .WithMany(s => s.Payments)
               .HasForeignKey(p => p.SaleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

