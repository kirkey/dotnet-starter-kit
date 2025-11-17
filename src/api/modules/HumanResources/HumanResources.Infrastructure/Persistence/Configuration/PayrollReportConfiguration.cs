using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configuration;

/// <summary>
/// Entity Type Configuration for PayrollReport.
/// Configures database mapping, indexes, and constraints for payroll report entities.
/// </summary>
public sealed class PayrollReportConfiguration : IEntityTypeConfiguration<PayrollReport>
{
    /// <summary>
    /// Configures the PayrollReport entity mapping.
    /// </summary>
    public void Configure(EntityTypeBuilder<PayrollReport> builder)
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

        builder.Property(x => x.PayrollPeriod)
            .HasMaxLength(50);

        builder.Property(x => x.TotalEmployees)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalPayrollRuns)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalGrossPay)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalNetPay)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalDeductions)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalTaxes)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalBenefits)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.AverageGrossPerEmployee)
            .HasPrecision(18, 2)
            .HasDefaultValue(0m);

        builder.Property(x => x.AverageNetPerEmployee)
            .HasPrecision(18, 2)
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
        builder.HasIndex(x => x.ReportType).HasDatabaseName("idx_payroll_report_type");
        builder.HasIndex(x => x.GeneratedOn).HasDatabaseName("idx_payroll_report_generated_on");
        builder.HasIndex(x => x.IsActive).HasDatabaseName("idx_payroll_report_is_active");
        builder.HasIndex(x => x.DepartmentId).HasDatabaseName("idx_payroll_report_department_id");
        builder.HasIndex(x => x.EmployeeId).HasDatabaseName("idx_payroll_report_employee_id");
        builder.HasIndex(x => x.PayrollPeriod).HasDatabaseName("idx_payroll_report_period");
        builder.HasIndex(x => new { x.FromDate, x.ToDate }).HasDatabaseName("idx_payroll_report_date_range");

        // Table configuration
        builder.ToTable(nameof(PayrollReport), SchemaNames.HumanResources);
    }
}

