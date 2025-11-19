namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an attendance report with aggregated attendance data for analysis.
/// Supports multiple report types: Summary, Daily, Monthly, Department, etc.
/// </summary>
public class AttendanceReport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Private parameterless constructor for EF Core.
    /// </summary>
    private AttendanceReport() { }

    /// <summary>
    /// Private constructor used by factory methods.
    /// </summary>
    private AttendanceReport(
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
    /// Gets the report type (Summary, Daily, Monthly, Department, EmployeeDetails, etc.).
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
    /// Gets the total number of working days in the period.
    /// </summary>
    public int TotalWorkingDays { get; private set; }

    /// <summary>
    /// Gets the total number of employees included in the report.
    /// </summary>
    public int TotalEmployees { get; private set; }

    /// <summary>
    /// Gets the total present count.
    /// </summary>
    public int PresentCount { get; private set; }

    /// <summary>
    /// Gets the total absent count.
    /// </summary>
    public int AbsentCount { get; private set; }

    /// <summary>
    /// Gets the total late count.
    /// </summary>
    public int LateCount { get; private set; }

    /// <summary>
    /// Gets the total half-day count.
    /// </summary>
    public int HalfDayCount { get; private set; }

    /// <summary>
    /// Gets the total on-leave count.
    /// </summary>
    public int OnLeaveCount { get; private set; }

    /// <summary>
    /// Gets the attendance percentage (0-100).
    /// </summary>
    public decimal AttendancePercentage { get; private set; }

    /// <summary>
    /// Gets the late percentage (0-100).
    /// </summary>
    public decimal LatePercentage { get; private set; }

    /// <summary>
    /// Gets the JSON data containing detailed report data.
    /// Stores detailed breakdown, per-employee metrics, daily summaries.
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
    /// Creates a new attendance report.
    /// </summary>
    /// <param name="reportType">Type of report to generate.</param>
    /// <param name="title">Descriptive title for the report.</param>
    /// <param name="fromDate">Start date of reporting period.</param>
    /// <param name="toDate">End date of reporting period.</param>
    /// <param name="departmentId">Optional department filter.</param>
    /// <param name="employeeId">Optional employee filter.</param>
    /// <returns>New AttendanceReport instance.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static AttendanceReport Create(
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

        var report = new AttendanceReport(reportType, title, fromDate, toDate)
        {
            DepartmentId = departmentId,
            EmployeeId = employeeId
        };

        return report;
    }

    /// <summary>
    /// Sets the aggregated attendance metrics for the report.
    /// </summary>
    public AttendanceReport SetMetrics(
        int workingDays,
        int employees,
        int present,
        int absent,
        int late,
        int halfDay,
        int onLeave)
    {
        TotalWorkingDays = workingDays;
        TotalEmployees = employees;
        PresentCount = present;
        AbsentCount = absent;
        LateCount = late;
        HalfDayCount = halfDay;
        OnLeaveCount = onLeave;

        // Calculate percentages
        if (employees > 0 && workingDays > 0)
        {
            var totalExpectedDays = employees * workingDays;
            var actualPresentDays = present + (halfDay / 2m); // Half days count as 0.5
            AttendancePercentage = Math.Round((actualPresentDays / totalExpectedDays) * 100, 2);
            LatePercentage = Math.Round(((decimal)late / (decimal)totalExpectedDays) * 100, 2);
        }

        return this;
    }

    /// <summary>
    /// Sets the report data (JSON content).
    /// </summary>
    public AttendanceReport SetReportData(string jsonData)
    {
        ReportData = jsonData;
        return this;
    }

    /// <summary>
    /// Sets the export path for the generated report file.
    /// </summary>
    public AttendanceReport SetExportPath(string path)
    {
        ExportPath = path;
        return this;
    }

    /// <summary>
    /// Adds notes or comments to the report.
    /// </summary>
    public AttendanceReport AddNotes(string notes)
    {
        Notes = notes;
        return this;
    }

    /// <summary>
    /// Updates the report status.
    /// </summary>
    public AttendanceReport SetActive(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}

