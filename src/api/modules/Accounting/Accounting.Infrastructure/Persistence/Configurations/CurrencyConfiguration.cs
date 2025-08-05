using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.CurrencyCode).IsUnique();

        builder.Property(x => x.CurrencyCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Symbol)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.DecimalPlaces)
            .IsRequired();

        builder.Property(x => x.IsBaseCurrency)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
