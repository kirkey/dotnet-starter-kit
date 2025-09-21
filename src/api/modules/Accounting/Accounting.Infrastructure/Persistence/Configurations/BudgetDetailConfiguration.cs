using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for <see cref="BudgetDetail"/> when treated as a first-class entity.
/// Maps properties, keys, indexes and precision to the underlying database table.
/// </summary>
public class BudgetDetailConfiguration : IEntityTypeConfiguration<BudgetDetail>
{
    public void Configure(EntityTypeBuilder<BudgetDetail> builder)
    {
        builder.ToTable("BudgetDetails", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BudgetId)
            .IsRequired();

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.BudgetedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasIndex(x => x.BudgetId);

        // Relationship to Budget is configured from BudgetConfiguration.HasMany(...).WithOne()
    }
}

