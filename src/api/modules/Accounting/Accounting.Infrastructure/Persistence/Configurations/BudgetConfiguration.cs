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

        // Configure BudgetDetails as a normal one-to-many relationship (entity) instead of owned
        builder.HasMany(x => x.BudgetDetails)
            .WithOne()
            .HasForeignKey(x => x.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Note: Property-level configuration for BudgetDetail is handled in BudgetDetailConfiguration
    }
}
