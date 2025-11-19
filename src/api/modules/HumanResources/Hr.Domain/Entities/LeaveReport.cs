namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a leave report with aggregated leave analytics for analysis.
/// Supports multiple report types: Summary, Detailed, Departmental, Trends, Balances.
/// </summary>
public class LeaveReport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Private parameterless constructor for EF Core.
    /// </summary>
    private LeaveReport() { }

    /// <summary>
    /// Private constructor used by factory methods.
    /// </summary>
    private LeaveReport(
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
    /// Gets the report type (Summary, Detailed, Departmental, Trends, Balances, EmployeeDetails).
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
    /// Gets the total number of employees with leave data.
    /// </summary>
    public int TotalEmployees { get; private set; }

    /// <summary>
    /// Gets the total number of leave types included.
    /// </summary>
    public int TotalLeaveTypes { get; private set; }

    /// <summary>
    /// Gets the total number of leave requests in period.
    /// </summary>
    public int TotalLeaveRequests { get; private set; }

    /// <summary>
    /// Gets the total approved leave count.
    /// </summary>
    public int ApprovedLeaveCount { get; private set; }

    /// <summary>
    /// Gets the total pending leave request count.
    /// </summary>
    public int PendingLeaveCount { get; private set; }

    /// <summary>
    /// Gets the total rejected leave count.
    /// </summary>
    public int RejectedLeaveCount { get; private set; }

    /// <summary>
    /// Gets the total leave days consumed.
    /// </summary>
    public decimal TotalLeaveConsumed { get; private set; }

    /// <summary>
    /// Gets the average leave per employee.
    /// </summary>
    public decimal AverageLeavePerEmployee { get; private set; }

    /// <summary>
    /// Gets the highest leave type count.
    /// </summary>
    public int HighestLeaveType { get; private set; }

    /// <summary>
    /// Gets the JSON data containing detailed report data.
    /// Stores leave breakdown by type, by department, by employee.
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
    /// Creates a new leave report.
    /// </summary>
    /// <param name="reportType">Type of report to generate.</param>
    /// <param name="title">Descriptive title for the report.</param>
    /// <param name="fromDate">Start date of reporting period.</param>
    /// <param name="toDate">End date of reporting period.</param>
    /// <param name="departmentId">Optional department filter.</param>
    /// <param name="employeeId">Optional employee filter.</param>
    /// <returns>New LeaveReport instance.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static LeaveReport Create(
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

        var report = new LeaveReport(reportType, title, fromDate, toDate)
        {
            DepartmentId = departmentId,
            EmployeeId = employeeId
        };

        return report;
    }

    /// <summary>
    /// Sets the aggregated leave metrics for the report.
    /// </summary>
    public LeaveReport SetMetrics(
        int employees,
        int leaveTypes,
        int totalRequests,
        int approved,
        int pending,
        int rejected,
        decimal consumed)
    {
        TotalEmployees = employees;
        TotalLeaveTypes = leaveTypes;
        TotalLeaveRequests = totalRequests;
        ApprovedLeaveCount = approved;
        PendingLeaveCount = pending;
        RejectedLeaveCount = rejected;
        TotalLeaveConsumed = consumed;

        if (employees > 0)
        {
            AverageLeavePerEmployee = Math.Round(consumed / employees, 2);
        }

        return this;
    }

    /// <summary>
    /// Sets the report data (JSON content).
    /// </summary>
    public LeaveReport SetReportData(string jsonData)
    {
        ReportData = jsonData;
        return this;
    }

    /// <summary>
    /// Sets the export path for the generated report file.
    /// </summary>
    public LeaveReport SetExportPath(string path)
    {
        ExportPath = path;
        return this;
    }

    /// <summary>
    /// Adds notes or comments to the report.
    /// </summary>
    public LeaveReport AddNotes(string notes)
    {
        Notes = notes;
        return this;
    }

    /// <summary>
    /// Updates the report status.
    /// </summary>
    public LeaveReport SetActive(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}

