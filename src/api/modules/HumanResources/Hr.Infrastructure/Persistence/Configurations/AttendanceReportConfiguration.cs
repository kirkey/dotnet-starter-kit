using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Type Configuration for AttendanceReport.
/// Configures database mapping, indexes, and constraints for attendance report entities.
/// </summary>
public sealed class AttendanceReportConfiguration : IEntityTypeConfiguration<AttendanceReport>
{
    /// <summary>
    /// Configures the AttendanceReport entity mapping.
    /// </summary>
    public void Configure(EntityTypeBuilder<AttendanceReport> builder)
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

        builder.Property(x => x.TotalWorkingDays)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalEmployees)
            .HasDefaultValue(0);

        builder.Property(x => x.PresentCount)
            .HasDefaultValue(0);

        builder.Property(x => x.AbsentCount)
            .HasDefaultValue(0);

        builder.Property(x => x.LateCount)
            .HasDefaultValue(0);

        builder.Property(x => x.HalfDayCount)
            .HasDefaultValue(0);

        builder.Property(x => x.OnLeaveCount)
            .HasDefaultValue(0);

        builder.Property(x => x.AttendancePercentage)
            .HasPrecision(5, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.LatePercentage)
            .HasPrecision(5, 2)
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
        builder.HasIndex(x => x.ReportType).HasDatabaseName("idx_attendance_report_type");
        builder.HasIndex(x => x.GeneratedOn).HasDatabaseName("idx_attendance_report_generated_on");
        builder.HasIndex(x => x.IsActive).HasDatabaseName("idx_attendance_report_is_active");
        builder.HasIndex(x => x.DepartmentId).HasDatabaseName("idx_attendance_report_department_id");
        builder.HasIndex(x => x.EmployeeId).HasDatabaseName("idx_attendance_report_employee_id");
        builder.HasIndex(x => new { x.FromDate, x.ToDate }).HasDatabaseName("idx_attendance_report_period");

        // Table configuration
        builder.ToTable(nameof(AttendanceReport), SchemaNames.HumanResources);
    }
}

