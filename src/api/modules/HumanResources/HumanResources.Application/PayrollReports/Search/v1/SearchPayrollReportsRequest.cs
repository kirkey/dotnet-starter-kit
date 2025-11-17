namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Search.v1;

/// <summary>
/// Query to search payroll reports with filters and pagination.
/// </summary>
public sealed class SearchPayrollReportsRequest : PaginationFilter, IRequest<PagedList<PayrollReportDto>>
{
    /// <summary>
    /// Optional report type filter.
    /// </summary>
    public string? ReportType { get; set; }

    /// <summary>
    /// Optional title search filter.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Optional department ID filter.
    /// </summary>
    public DefaultIdType? DepartmentId { get; set; }

    /// <summary>
    /// Optional employee ID filter.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Optional payroll period filter.
    /// </summary>
    public string? PayrollPeriod { get; set; }

    /// <summary>
    /// Optional active status filter.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Optional from date for generated date range filter.
    /// </summary>
    public DateTime? GeneratedFrom { get; set; }

    /// <summary>
    /// Optional to date for generated date range filter.
    /// </summary>
    public DateTime? GeneratedTo { get; set; }
}

/// <summary>
/// DTO for payroll report search results.
/// </summary>
public sealed record PayrollReportDto(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    int TotalEmployees,
    int TotalPayrollRuns,
    decimal TotalGrossPay,
    decimal TotalNetPay,
    string? ExportPath,
    bool IsActive);

