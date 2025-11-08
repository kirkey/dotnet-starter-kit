namespace Accounting.Infrastructure.Persistence.Configurations;

public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
{
    public void Configure(EntityTypeBuilder<CostCenter> builder)
    {
        builder.ToTable("CostCenters", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code).IsUnique();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.CostCenterType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.ParentCostCenterId);

        builder.Property(x => x.ManagerId);

        builder.Property(x => x.ManagerName)
            .HasMaxLength(256);

        builder.Property(x => x.BudgetAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ActualAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Location)
            .HasMaxLength(256);

        builder.Property(x => x.StartDate);

        builder.Property(x => x.EndDate);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // Single column indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_CostCenters_Code");
        
        builder.HasIndex(x => x.ParentCostCenterId)
            .HasDatabaseName("IX_CostCenters_ParentCostCenterId");
        
        builder.HasIndex(x => x.ManagerId)
            .HasDatabaseName("IX_CostCenters_ManagerId");
        
        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_CostCenters_IsActive");
        
        builder.HasIndex(x => x.CostCenterType)
            .HasDatabaseName("IX_CostCenters_CostCenterType");
        
        builder.HasIndex(x => x.StartDate)
            .HasDatabaseName("IX_CostCenters_StartDate");
        
        builder.HasIndex(x => x.EndDate)
            .HasDatabaseName("IX_CostCenters_EndDate");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.IsActive, x.CostCenterType })
            .HasDatabaseName("IX_CostCenters_IsActive_Type");
        
        builder.HasIndex(x => new { x.ParentCostCenterId, x.IsActive })
            .HasDatabaseName("IX_CostCenters_Parent_IsActive");
        
        builder.HasIndex(x => new { x.ManagerId, x.IsActive })
            .HasDatabaseName("IX_CostCenters_Manager_IsActive");
        
        // Composite index for budget variance reports
        builder.HasIndex(x => new { x.IsActive, x.BudgetAmount, x.ActualAmount })
            .HasDatabaseName("IX_CostCenters_Active_Budget_Actual");
        
        // Composite index for period-based queries
        builder.HasIndex(x => new { x.StartDate, x.EndDate, x.IsActive })
            .HasDatabaseName("IX_CostCenters_DateRange_IsActive");
    }
}
