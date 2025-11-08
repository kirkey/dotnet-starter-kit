namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for FiscalPeriodClose entity.
/// </summary>
public class FiscalPeriodCloseConfiguration : IEntityTypeConfiguration<FiscalPeriodClose>
{
    public void Configure(EntityTypeBuilder<FiscalPeriodClose> builder)
    {
        builder.ToTable("FiscalPeriodCloses", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CloseNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.CloseType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.InitiatedBy).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.CompletedBy).HasMaxLength(256);
        builder.Property(x => x.ReopenReason).HasMaxLength(1000);
        builder.Property(x => x.ReopenedBy).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.FinalNetIncome).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.CloseNumber)
            .IsUnique()
            .HasDatabaseName("IX_FiscalPeriodCloses_CloseNumber");

        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_FiscalPeriodCloses_PeriodId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FiscalPeriodCloses_Status");

        builder.HasIndex(x => x.CloseType)
            .HasDatabaseName("IX_FiscalPeriodCloses_CloseType");

        builder.HasIndex(x => x.InitiatedBy)
            .HasDatabaseName("IX_FiscalPeriodCloses_InitiatedBy");

        builder.HasIndex(x => x.CloseInitiatedDate)
            .HasDatabaseName("IX_FiscalPeriodCloses_CloseInitiatedDate");

        builder.HasIndex(x => x.CompletedDate)
            .HasDatabaseName("IX_FiscalPeriodCloses_CompletedDate");

        builder.HasIndex(x => x.CompletedBy)
            .HasDatabaseName("IX_FiscalPeriodCloses_CompletedBy");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.Status, x.CloseInitiatedDate })
            .HasDatabaseName("IX_FiscalPeriodCloses_Status_CloseInitiatedDate");

        builder.HasIndex(x => new { x.PeriodId, x.Status })
            .HasDatabaseName("IX_FiscalPeriodCloses_Period_Status");

        builder.HasIndex(x => new { x.CloseType, x.Status })
            .HasDatabaseName("IX_FiscalPeriodCloses_CloseType_Status");

        builder.HasIndex(x => new { x.PeriodStartDate, x.PeriodEndDate })
            .HasDatabaseName("IX_FiscalPeriodCloses_PeriodRange");
        
        builder.OwnsMany(x => x.Tasks, taskBuilder =>
        {
            taskBuilder.ToTable("FiscalPeriodCloseTasks", SchemaNames.Accounting);
            taskBuilder.WithOwner().HasForeignKey("FiscalPeriodCloseId");
            taskBuilder.Property<int>("Id");
            taskBuilder.HasKey("Id");
            taskBuilder.Property(t => t.TaskName).IsRequired().HasMaxLength(256);

            // Indexes for tasks
            taskBuilder.HasIndex("FiscalPeriodCloseId")
                .HasDatabaseName("IX_FiscalPeriodCloseTasks_CloseId");

            taskBuilder.HasIndex(t => t.TaskName)
                .HasDatabaseName("IX_FiscalPeriodCloseTasks_TaskName");

            taskBuilder.HasIndex(t => t.IsComplete)
                .HasDatabaseName("IX_FiscalPeriodCloseTasks_IsComplete");
        });
        
        builder.OwnsMany(x => x.ValidationIssues, issue =>
        {
            issue.ToTable("FiscalPeriodCloseValidationIssues", SchemaNames.Accounting);
            issue.WithOwner().HasForeignKey("FiscalPeriodCloseId");
            issue.Property<int>("Id");
            issue.HasKey("Id");
            issue.Property(i => i.IssueDescription).IsRequired().HasMaxLength(1000);
            issue.Property(i => i.Severity).IsRequired().HasMaxLength(32);
            issue.Property(i => i.Resolution).HasMaxLength(2000);

            // Indexes for validation issues
            issue.HasIndex("FiscalPeriodCloseId")
                .HasDatabaseName("IX_FiscalPeriodCloseValidationIssues_CloseId");

            issue.HasIndex(i => i.Severity)
                .HasDatabaseName("IX_FiscalPeriodCloseValidationIssues_Severity");

            issue.HasIndex(i => i.IsResolved)
                .HasDatabaseName("IX_FiscalPeriodCloseValidationIssues_IsResolved");
        });
    }
}
