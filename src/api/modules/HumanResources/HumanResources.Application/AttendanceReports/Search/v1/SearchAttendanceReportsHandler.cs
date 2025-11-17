namespace FSH.Starter.WebApi.HumanResources.Application.AttendanceReports.Search.v1;

/// <summary>
/// Handler for searching attendance reports.
/// </summary>
public sealed class SearchAttendanceReportsHandler(
    ILogger<SearchAttendanceReportsHandler> logger,
    [FromKeyedServices("hr:attendancereports")] IReadRepository<AttendanceReport> repository)
    : IRequestHandler<SearchAttendanceReportsRequest, PagedList<AttendanceReportDto>>
{
    /// <summary>
    /// Handles the search attendance reports query.
    /// </summary>
    public async Task<PagedList<AttendanceReportDto>> Handle(
        SearchAttendanceReportsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAttendanceReportsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Retrieved {Count} attendance reports from total {Total} matching filters",
            items.Count,
            totalCount);

        return new PagedList<AttendanceReportDto>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

