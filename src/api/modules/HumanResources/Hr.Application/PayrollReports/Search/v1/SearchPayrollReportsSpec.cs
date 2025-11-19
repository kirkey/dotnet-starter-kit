namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Search.v1;

/// <summary>
/// Specification for searching payroll reports with filters.
/// </summary>
public sealed class SearchPayrollReportsSpec : EntitiesByPaginationFilterSpec<PayrollReport, PayrollReportDto>
{
    /// <summary>
    /// Initializes a new instance of the specification.
    /// </summary>
    public SearchPayrollReportsSpec(SearchPayrollReportsRequest request)
        : base(request) =>
        Query
            .OrderByDescending(x => x.GeneratedOn, !request.HasOrderBy())
            .Where(x => x.ReportType == request.ReportType, !string.IsNullOrWhiteSpace(request.ReportType))
            .Where(x => x.Title.Contains(request.Title!), !string.IsNullOrWhiteSpace(request.Title))
            .Where(x => x.DepartmentId == request.DepartmentId, request.DepartmentId.HasValue)
            .Where(x => x.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(x => x.PayrollPeriod == request.PayrollPeriod, !string.IsNullOrWhiteSpace(request.PayrollPeriod))
            .Where(x => x.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(x => x.GeneratedOn >= request.GeneratedFrom, request.GeneratedFrom.HasValue)
            .Where(x => x.GeneratedOn <= request.GeneratedTo, request.GeneratedTo.HasValue);
}

