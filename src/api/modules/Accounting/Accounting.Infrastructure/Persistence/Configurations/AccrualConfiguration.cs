namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccrualConfiguration : IEntityTypeConfiguration<Accrual>
{
    public void Configure(EntityTypeBuilder<Accrual> builder)
    {
        builder.ToTable("Accruals", SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.AccrualNumber).IsUnique();

        builder.Property(x => x.AccrualNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.AccrualDate)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IsReversed)
            .IsRequired();

        builder.Property(x => x.ReversalDate);
        
        // Single column indexes
        builder.HasIndex(x => x.AccrualNumber)
            .IsUnique()
            .HasDatabaseName("IX_Accruals_AccrualNumber");
        
        builder.HasIndex(x => x.AccrualDate)
            .HasDatabaseName("IX_Accruals_AccrualDate");
        
        builder.HasIndex(x => x.IsReversed)
            .HasDatabaseName("IX_Accruals_IsReversed");
        
        builder.HasIndex(x => x.ReversalDate)
            .HasDatabaseName("IX_Accruals_ReversalDate");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Accruals_Status");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.IsReversed, x.AccrualDate })
            .HasDatabaseName("IX_Accruals_IsReversed_Date");
        
        builder.HasIndex(x => new { x.Status, x.AccrualDate })
            .HasDatabaseName("IX_Accruals_Status_Date");
        
        // Composite index for reversal tracking
        builder.HasIndex(x => new { x.IsReversed, x.ReversalDate, x.Status })
            .HasDatabaseName("IX_Accruals_IsReversed_ReversalDate_Status");
    }
}

