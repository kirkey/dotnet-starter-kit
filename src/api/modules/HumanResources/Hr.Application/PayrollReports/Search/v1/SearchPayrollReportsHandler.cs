namespace FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Search.v1;

/// <summary>
/// Handler for searching payroll reports.
/// </summary>
public sealed class SearchPayrollReportsHandler(
    ILogger<SearchPayrollReportsHandler> logger,
    [FromKeyedServices("hr:payrollreports")] IReadRepository<PayrollReport> repository)
    : IRequestHandler<SearchPayrollReportsRequest, PagedList<PayrollReportDto>>
{
    /// <summary>
    /// Handles the search payroll reports query.
    /// </summary>
    public async Task<PagedList<PayrollReportDto>> Handle(
        SearchPayrollReportsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPayrollReportsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Retrieved {Count} payroll reports from total {Total} matching filters",
            items.Count,
            totalCount);

        return new PagedList<PayrollReportDto>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

