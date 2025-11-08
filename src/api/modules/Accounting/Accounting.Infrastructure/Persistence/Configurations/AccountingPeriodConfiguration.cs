namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriod>
{
    public void Configure(EntityTypeBuilder<AccountingPeriod> builder)
    {
        builder.ToTable("AccountingPeriods", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.FiscalYear, x.PeriodType }).IsUnique();

        // Name, Description, Notes - align lengths with domain/AuditableEntity
        builder.Property(x => x.Name)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        builder.Property(x => x.FiscalYear)
            .IsRequired();

        builder.Property(x => x.PeriodType)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.IsClosed)
            .IsRequired();

        builder.Property(x => x.IsAdjustmentPeriod)
            .IsRequired();

        // Indexes for query optimization
        builder.HasIndex(x => new { x.FiscalYear, x.PeriodType })
            .IsUnique()
            .HasDatabaseName("IX_AccountingPeriods_FiscalYear_PeriodType");
        
        builder.HasIndex(x => x.FiscalYear)
            .HasDatabaseName("IX_AccountingPeriods_FiscalYear");
        
        builder.HasIndex(x => x.PeriodType)
            .HasDatabaseName("IX_AccountingPeriods_PeriodType");
        
        builder.HasIndex(x => x.IsClosed)
            .HasDatabaseName("IX_AccountingPeriods_IsClosed");
        
        builder.HasIndex(x => x.IsAdjustmentPeriod)
            .HasDatabaseName("IX_AccountingPeriods_IsAdjustmentPeriod");
        
        builder.HasIndex(x => x.StartDate)
            .HasDatabaseName("IX_AccountingPeriods_StartDate");
        
        builder.HasIndex(x => x.EndDate)
            .HasDatabaseName("IX_AccountingPeriods_EndDate");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.StartDate, x.EndDate })
            .HasDatabaseName("IX_AccountingPeriods_DateRange");
        
        builder.HasIndex(x => new { x.IsClosed, x.FiscalYear })
            .HasDatabaseName("IX_AccountingPeriods_IsClosed_Year");
        
        builder.HasIndex(x => new { x.FiscalYear, x.IsClosed, x.PeriodType })
            .HasDatabaseName("IX_AccountingPeriods_Year_Closed_Type");
        
        // Composite index for period selection queries
        builder.HasIndex(x => new { x.StartDate, x.EndDate, x.IsClosed })
            .HasDatabaseName("IX_AccountingPeriods_DateRange_IsClosed");
        
        // Composite index for adjustment period queries
        builder.HasIndex(x => new { x.FiscalYear, x.IsAdjustmentPeriod, x.IsClosed })
            .HasDatabaseName("IX_AccountingPeriods_Year_Adjustment_Closed");
    }
}
