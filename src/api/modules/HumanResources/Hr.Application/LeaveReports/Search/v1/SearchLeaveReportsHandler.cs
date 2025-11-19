namespace FSH.Starter.WebApi.HumanResources.Application.LeaveReports.Search.v1;

/// <summary>
/// Handler for searching leave reports.
/// </summary>
public sealed class SearchLeaveReportsHandler(
    ILogger<SearchLeaveReportsHandler> logger,
    [FromKeyedServices("hr:leavereports")] IReadRepository<LeaveReport> repository)
    : IRequestHandler<SearchLeaveReportsRequest, PagedList<LeaveReportDto>>
{
    /// <summary>
    /// Handles the search leave reports query.
    /// </summary>
    public async Task<PagedList<LeaveReportDto>> Handle(
        SearchLeaveReportsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchLeaveReportsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Retrieved {Count} leave reports from total {Total} matching filters",
            items.Count,
            totalCount);

        return new PagedList<LeaveReportDto>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

