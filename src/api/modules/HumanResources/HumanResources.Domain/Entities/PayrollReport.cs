namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a payroll report with consolidated payroll data for analysis and export.
/// Supports multiple report types: Summary, Detailed, Department, etc.
/// </summary>
public class PayrollReport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Private parameterless constructor for EF Core.
    /// </summary>
    private PayrollReport() { }

    /// <summary>
    /// Private constructor used by factory methods.
    /// </summary>
    private PayrollReport(
        string reportType,
        string title,
        DateTime fromDate,
        DateTime toDate)
    {
        ReportType = reportType;
        Title = title;
        FromDate = fromDate;
        ToDate = toDate;
        GeneratedOn = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Gets the report type (Summary, Detailed, Department, EmployeeDetails, TaxSummary, etc.).
    /// </summary>
    public string ReportType { get; private set; } = default!;

    /// <summary>
    /// Gets the descriptive title of the report.
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// Gets the start date of the reporting period.
    /// </summary>
    public DateTime FromDate { get; private set; }

    /// <summary>
    /// Gets the end date of the reporting period.
    /// </summary>
    public DateTime ToDate { get; private set; }

    /// <summary>
    /// Gets the date and time the report was generated.
    /// </summary>
    public DateTime GeneratedOn { get; private set; }

    /// <summary>
    /// Gets the optional department ID to filter report data.
    /// Null for company-wide reports.
    /// </summary>
    public DefaultIdType? DepartmentId { get; private set; }

    /// <summary>
    /// Gets the optional employee ID for employee-specific reports.
    /// Null for aggregate reports.
    /// </summary>
    public DefaultIdType? EmployeeId { get; private set; }

    /// <summary>
    /// Gets the total number of records in the report.
    /// </summary>
    public int RecordCount { get; private set; }

    /// <summary>
    /// Gets the total gross salary amount in the report.
    /// </summary>
    public decimal TotalGrossSalary { get; private set; }

    /// <summary>
    /// Gets the total deductions amount in the report.
    /// </summary>
    public decimal TotalDeductions { get; private set; }

    /// <summary>
    /// Gets the total net salary amount in the report.
    /// </summary>
    public decimal TotalNetSalary { get; private set; }

    /// <summary>
    /// Gets the total tax amount in the report.
    /// </summary>
    public decimal TotalTax { get; private set; }

    /// <summary>
    /// Gets the JSON data containing the actual report data.
    /// Stores flattened report structure for flexibility.
    /// </summary>
    public string? ReportData { get; private set; }

    /// <summary>
    /// Gets the file path or URL where the report was exported (if applicable).
    /// </summary>
    public string? ExportPath { get; private set; }

    /// <summary>
    /// Gets the optional notes or comments about this report.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the report is active and available for viewing.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new payroll report.
    /// </summary>
    /// <param name="reportType">Type of report to generate.</param>
    /// <param name="title">Descriptive title for the report.</param>
    /// <param name="fromDate">Start date of reporting period.</param>
    /// <param name="toDate">End date of reporting period.</param>
    /// <param name="departmentId">Optional department filter.</param>
    /// <param name="employeeId">Optional employee filter.</param>
    /// <returns>New PayrollReport instance.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static PayrollReport Create(
        string reportType,
        string title,
        DateTime fromDate,
        DateTime toDate,
        DefaultIdType? departmentId = null,
        DefaultIdType? employeeId = null)
    {
        if (string.IsNullOrWhiteSpace(reportType))
            throw new ArgumentException("Report type cannot be empty", nameof(reportType));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Report title cannot be empty", nameof(title));
        if (toDate < fromDate)
            throw new ArgumentException("End date must be after start date", nameof(toDate));

        var report = new PayrollReport(reportType, title, fromDate, toDate)
        {
            DepartmentId = departmentId,
            EmployeeId = employeeId
        };

        return report;
    }

    /// <summary>
    /// Sets the aggregated totals for the report.
    /// </summary>
    public PayrollReport SetTotals(
        int recordCount,
        decimal grossSalary,
        decimal deductions,
        decimal netSalary,
        decimal tax)
    {
        RecordCount = recordCount;
        TotalGrossSalary = grossSalary;
        TotalDeductions = deductions;
        TotalNetSalary = netSalary;
        TotalTax = tax;

        return this;
    }

    /// <summary>
    /// Sets the report data (JSON content).
    /// </summary>
    public PayrollReport SetReportData(string jsonData)
    {
        ReportData = jsonData;
        return this;
    }

    /// <summary>
    /// Sets the export path for the generated report file.
    /// </summary>
    public PayrollReport SetExportPath(string path)
    {
        ExportPath = path;
        return this;
    }

    /// <summary>
    /// Adds notes or comments to the report.
    /// </summary>
    public PayrollReport AddNotes(string notes)
    {
        Notes = notes;
        return this;
    }

    /// <summary>
    /// Updates the report status.
    /// </summary>
    public PayrollReport SetActive(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}

