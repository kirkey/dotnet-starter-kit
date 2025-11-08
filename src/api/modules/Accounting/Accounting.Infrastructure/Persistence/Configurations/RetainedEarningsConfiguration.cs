namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for RetainedEarnings entity.
/// </summary>
public class RetainedEarningsConfiguration : IEntityTypeConfiguration<RetainedEarnings>
{
    public void Configure(EntityTypeBuilder<RetainedEarnings> builder)
    {
        builder.ToTable("RetainedEarnings", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.ClosedBy).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.OpeningBalance).HasPrecision(18, 2);
        builder.Property(x => x.NetIncome).HasPrecision(18, 2);
        builder.Property(x => x.Distributions).HasPrecision(18, 2);
        builder.Property(x => x.CapitalContributions).HasPrecision(18, 2);
        builder.Property(x => x.OtherEquityChanges).HasPrecision(18, 2);
        builder.Property(x => x.ClosingBalance).HasPrecision(18, 2);
        builder.Property(x => x.ApproprietedAmount).HasPrecision(18, 2);
        builder.Property(x => x.UnappropriatedAmount).HasPrecision(18, 2);
        
        // Single column indexes
        builder.HasIndex(x => x.FiscalYear)
            .IsUnique()
            .HasDatabaseName("IX_RetainedEarnings_FiscalYear");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_RetainedEarnings_Status");
        
        builder.HasIndex(x => x.ClosedDate)
            .HasDatabaseName("IX_RetainedEarnings_ClosedDate");
        
        builder.HasIndex(x => x.ClosedBy)
            .HasDatabaseName("IX_RetainedEarnings_ClosedBy");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.FiscalYear, x.Status })
            .HasDatabaseName("IX_RetainedEarnings_Year_Status");
        
        builder.HasIndex(x => new { x.Status, x.ClosedDate })
            .HasDatabaseName("IX_RetainedEarnings_Status_ClosedDate");
        
        builder.HasIndex(x => new { x.FiscalYear, x.ClosingBalance })
            .HasDatabaseName("IX_RetainedEarnings_Year_ClosingBalance");
        
        // Composite index for equity change analysis
        builder.HasIndex(x => new { x.FiscalYear, x.NetIncome, x.Distributions })
            .HasDatabaseName("IX_RetainedEarnings_Year_Income_Distributions");
        
        // Composite index for appropriation tracking
        builder.HasIndex(x => new { x.FiscalYear, x.ApproprietedAmount, x.UnappropriatedAmount })
            .HasDatabaseName("IX_RetainedEarnings_Year_Appropriation");
    }
}

