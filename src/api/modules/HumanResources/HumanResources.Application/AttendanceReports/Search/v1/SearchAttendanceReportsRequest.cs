namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Search.v1;

/// <summary>
/// Query to search attendance reports with filters and pagination.
/// </summary>
public sealed class SearchAttendanceReportsRequest : PaginationFilter, IRequest<PagedList<AttendanceReportDto>>
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
    /// Optional minimum attendance percentage filter.
    /// </summary>
    public decimal? MinAttendancePercentage { get; set; }
}

/// <summary>
/// DTO for attendance report search results.
/// </summary>
public sealed record AttendanceReportDto(
    DefaultIdType Id,
    string ReportType,
    string Title,
    DateTime FromDate,
    DateTime ToDate,
    DateTime GeneratedOn,
    int TotalWorkingDays,
    int TotalEmployees,
    int PresentCount,
    int AbsentCount,
    decimal AttendancePercentage,
    string? ExportPath,
    bool IsActive);

