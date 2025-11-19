namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Search.v1;

/// <summary>
/// Specification for searching leave reports with filters.
/// </summary>
public sealed class SearchLeaveReportsSpec : EntitiesByPaginationFilterSpec<LeaveReport, LeaveReportDto>
{
    /// <summary>
    /// Initializes a new instance of the specification.
    /// </summary>
    public SearchLeaveReportsSpec(SearchLeaveReportsRequest request)
        : base(request) =>
        Query
            .OrderByDescending(x => x.GeneratedOn, !request.HasOrderBy())
            .Where(x => x.ReportType == request.ReportType, !string.IsNullOrWhiteSpace(request.ReportType))
            .Where(x => x.Title.Contains(request.Title!), !string.IsNullOrWhiteSpace(request.Title))
            .Where(x => x.DepartmentId == request.DepartmentId, request.DepartmentId.HasValue)
            .Where(x => x.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(x => x.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(x => x.GeneratedOn >= request.GeneratedFrom, request.GeneratedFrom.HasValue)
            .Where(x => x.GeneratedOn <= request.GeneratedTo, request.GeneratedTo.HasValue)
            .Where(x => x.PendingLeaveCount >= request.MinPendingRequests, request.MinPendingRequests.HasValue);
}

