using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();

        builder.HasKey(t => t.Id);

        builder.Property(t => t.PeriodType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.ManagerComment)
            .HasMaxLength(500);

        builder.Property(t => t.RejectionReason)
            .HasMaxLength(500);

        builder.Property(t => t.RegularHours)
            .HasPrecision(6, 2);

        builder.Property(t => t.OvertimeHours)
            .HasPrecision(6, 2);

        builder.Property(t => t.TotalHours)
            .HasPrecision(6, 2);

        builder.HasOne(t => t.Employee)
            .WithMany(e => e.Timesheets)
            .HasForeignKey(t => t.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Lines)
            .WithOne(l => l.Timesheet)
            .HasForeignKey(l => l.TimesheetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.EmployeeId)
            .HasDatabaseName("IX_Timesheet_EmployeeId");

        builder.HasIndex(t => t.StartDate)
            .HasDatabaseName("IX_Timesheet_StartDate");

        builder.HasIndex(t => new { t.EmployeeId, t.StartDate, t.EndDate })
            .HasDatabaseName("IX_Timesheet_EmployeeId_Period")
            .IsUnique();

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Timesheet_Status");

        builder.HasIndex(t => t.IsApproved)
            .HasDatabaseName("IX_Timesheet_IsApproved");

        builder.HasIndex(t => t.IsLocked)
            .HasDatabaseName("IX_Timesheet_IsLocked");

        // Optimized for approval/lock status filtering
        builder.HasIndex(t => new { t.Status, t.IsApproved, t.IsLocked, t.EmployeeId })
            .HasDatabaseName("IX_Timesheet_Status_Approval_Lock");

        // Period query optimization
        builder.HasIndex(t => new { t.StartDate, t.EndDate, t.EmployeeId, t.Status })
            .HasDatabaseName("IX_Timesheet_Period_Status");

        // Review workflow - pending approvals
        builder.HasIndex(t => new { t.IsApproved, t.Status })
            .HasDatabaseName("IX_Timesheet_Pending_Approvals");
    }
}

public class TimesheetLineConfiguration : IEntityTypeConfiguration<TimesheetLine>
{
    public void Configure(EntityTypeBuilder<TimesheetLine> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();

        builder.HasKey(l => l.Id);

        builder.Property(l => l.RegularHours)
            .HasPrecision(5, 2);

        builder.Property(l => l.OvertimeHours)
            .HasPrecision(5, 2);

        builder.Property(l => l.ProjectId)
            .HasMaxLength(50);

        builder.Property(l => l.TaskDescription)
            .HasMaxLength(500);

        builder.Property(l => l.BillingRate)
            .HasPrecision(10, 2)
            .IsRequired(false);

        builder.HasOne(l => l.Timesheet)
            .WithMany(t => t.Lines)
            .HasForeignKey(l => l.TimesheetId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => l.TimesheetId)
            .HasDatabaseName("IX_TimesheetLine_TimesheetId");

        builder.HasIndex(l => l.WorkDate)
            .HasDatabaseName("IX_TimesheetLine_WorkDate");

        builder.HasIndex(l => l.IsBillable)
            .HasDatabaseName("IX_TimesheetLine_IsBillable");

        // Optimized for billable hour queries
        builder.HasIndex(l => new { l.IsBillable, l.WorkDate })
            .HasDatabaseName("IX_TimesheetLine_Billable_Date");

        // Project-based hour tracking
        builder.HasIndex(l => new { l.ProjectId, l.WorkDate, l.IsBillable })
            .HasDatabaseName("IX_TimesheetLine_Project_Tracking");

        // Timesheet completion optimization
        builder.HasIndex(l => new { l.TimesheetId, l.WorkDate, l.IsBillable })
            .HasDatabaseName("IX_TimesheetLine_Completion");
    }
}

