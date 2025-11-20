using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Type Configuration for LeaveReport.
/// Configures database mapping, indexes, and constraints for leave report entities.
/// </summary>
public sealed class LeaveReportConfiguration : IEntityTypeConfiguration<LeaveReport>
{
    /// <summary>
    /// Configures the LeaveReport entity mapping.
    /// </summary>
    public void Configure(EntityTypeBuilder<LeaveReport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReportType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.FromDate)
            .IsRequired();

        builder.Property(x => x.ToDate)
            .IsRequired();

        builder.Property(x => x.GeneratedOn)
            .IsRequired();

        builder.Property(x => x.DepartmentId);

        builder.Property(x => x.EmployeeId);

        builder.Property(x => x.TotalEmployees)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalLeaveTypes)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalLeaveRequests)
            .HasDefaultValue(0);

        builder.Property(x => x.ApprovedLeaveCount)
            .HasDefaultValue(0);

        builder.Property(x => x.PendingLeaveCount)
            .HasDefaultValue(0);

        builder.Property(x => x.RejectedLeaveCount)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalLeaveConsumed)
            .HasPrecision(8, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.AverageLeavePerEmployee)
            .HasPrecision(8, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.ReportData)
            .HasColumnType("jsonb");

        builder.Property(x => x.ExportPath)
            .HasMaxLength(500);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Add indexes for common queries
        builder.HasIndex(x => x.ReportType).HasDatabaseName("idx_leave_report_type");
        builder.HasIndex(x => x.GeneratedOn).HasDatabaseName("idx_leave_report_generated_on");
        builder.HasIndex(x => x.IsActive).HasDatabaseName("idx_leave_report_is_active");
        builder.HasIndex(x => x.DepartmentId).HasDatabaseName("idx_leave_report_department_id");
        builder.HasIndex(x => x.EmployeeId).HasDatabaseName("idx_leave_report_employee_id");
        builder.HasIndex(x => new { x.FromDate, x.ToDate }).HasDatabaseName("idx_leave_report_period");

        // Table configuration
        builder.ToTable(nameof(LeaveReport), SchemaNames.HumanResources);
    }
}

