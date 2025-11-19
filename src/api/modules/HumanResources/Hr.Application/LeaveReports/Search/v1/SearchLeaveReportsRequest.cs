namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Search.v1;

/// <summary>
/// Query to search leave reports with filters and pagination.
/// </summary>
public sealed class SearchLeaveReportsRequest : PaginationFilter, IRequest<PagedList<LeaveReportDto>>
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

    /// <summary>
    /// Optional minimum pending leave requests filter.
    /// </summary>
    public int? MinPendingRequests { get; set; }
}

/// <summary>
/// DTO for leave report search results.
/// </summary>
public sealed record LeaveReportDto(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    int TotalEmployees,
    int TotalLeaveRequests,
    int ApprovedLeaveCount,
    int PendingLeaveCount,
    decimal TotalLeaveConsumed,
    string? ExportPath,
    bool IsActive);

