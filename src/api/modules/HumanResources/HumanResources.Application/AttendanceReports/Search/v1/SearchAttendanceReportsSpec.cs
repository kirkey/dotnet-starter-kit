namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Search.v1;

/// <summary>
/// Specification for searching attendance reports with filters.
/// </summary>
public sealed class SearchAttendanceReportsSpec : EntitiesByPaginationFilterSpec<AttendanceReport, AttendanceReportDto>
{
    /// <summary>
    /// Initializes a new instance of the specification.
    /// </summary>
    public SearchAttendanceReportsSpec(SearchAttendanceReportsRequest request)
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
            .Where(x => x.AttendancePercentage >= request.MinAttendancePercentage, request.MinAttendancePercentage.HasValue);
}

