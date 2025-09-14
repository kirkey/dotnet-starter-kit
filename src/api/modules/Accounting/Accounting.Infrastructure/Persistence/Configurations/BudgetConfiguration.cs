using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Unique constraint: Name + PeriodId (one budget name per period)
        builder.HasIndex(x => new { x.Name, x.PeriodId }).IsUnique();

        // Name - align with application validators (max 256)
        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.PeriodId)
            .IsRequired();

        builder.Property(x => x.FiscalYear)
            .IsRequired();

        builder.Property(x => x.BudgetType)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.TotalBudgetedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalActualAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Description and Notes - align with validator (max 1000)
        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        // Configure owned entity for BudgetLines
        builder.OwnsMany(x => x.BudgetLines, bl =>
        {
            bl.ToTable("BudgetLines", schema: SchemaNames.Accounting);
            bl.WithOwner().HasForeignKey("BudgetId");
            bl.HasKey("Id");
            
            bl.Property(x => x.AccountId)
                .IsRequired();
                
            bl.Property(x => x.BudgetedAmount)
                .HasPrecision(18, 2)
                .IsRequired();
                
            bl.Property(x => x.ActualAmount)
                .HasPrecision(18, 2)
                .IsRequired();
                
            // Keep BudgetLine description max 500 (application validators can be tightened if needed)
            bl.Property(x => x.Description)
                .HasMaxLength(500);
        });
    }
}
