using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class TaxBracketConfiguration : IEntityTypeConfiguration<TaxBracket>
{
    public void Configure(EntityTypeBuilder<TaxBracket> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaxType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.MinIncome)
            .HasPrecision(10, 2);

        builder.Property(t => t.MaxIncome)
            .HasPrecision(10, 2);

        builder.Property(t => t.Rate)
            .HasPrecision(5, 4);

        builder.Property(t => t.FilingStatus)
            .HasMaxLength(50);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.HasIndex(t => new { t.TaxType, t.Year })
            .HasDatabaseName("IX_TaxBracket_TaxType_Year");
    }
}

