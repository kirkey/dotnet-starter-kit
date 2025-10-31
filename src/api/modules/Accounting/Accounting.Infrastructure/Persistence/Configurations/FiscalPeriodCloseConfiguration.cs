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
        
        builder.HasIndex(x => x.CloseNumber).IsUnique();
        builder.HasIndex(x => x.PeriodId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CloseType);
        
        builder.OwnsMany(x => x.Tasks, task =>
        {
            task.ToTable("FiscalPeriodCloseTasks", SchemaNames.Accounting);
            task.WithOwner().HasForeignKey("FiscalPeriodCloseId");
            task.Property<int>("Id");
            task.HasKey("Id");
            task.Property(t => t.TaskName).IsRequired().HasMaxLength(256);
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
        });
    }
}
