using Accounting.Application.RegulatoryReports.Dtos;

namespace Accounting.Application.RegulatoryReports.Search.v1;

public sealed class SearchRegulatoryReportsRequestHandler(
    [FromKeyedServices("accounting:regulatoryreports")] IReadRepository<RegulatoryReport> repository)
    : IRequestHandler<SearchRegulatoryReportsRequest, PagedList<RegulatoryReportDto>>
{
    public async Task<PagedList<RegulatoryReportDto>> Handle(SearchRegulatoryReportsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRegulatoryReportsSpec(request);
        var allReports = await repository.ListAsync(spec, cancellationToken);

        var pagedReports = allReports
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var totalCount = allReports.Count;

        return new PagedList<RegulatoryReportDto>(
            pagedReports.Adapt<List<RegulatoryReportDto>>(),
            request.PageNumber,
            request.PageSize,
            totalCount
        );
    }
}

public class SearchRegulatoryReportsSpec : EntitiesByPaginationFilterSpec<RegulatoryReport, RegulatoryReportDto>
{
    public SearchRegulatoryReportsSpec(SearchRegulatoryReportsRequest request)
        : base(request) =>
        Query
            .OrderBy(r => r.DueDate, !request.HasOrderBy())
            .Where(r => r.ReportType.Contains(request.ReportType!), !string.IsNullOrEmpty(request.ReportType))
            .Where(r => r.Status.Contains(request.Status!), !string.IsNullOrEmpty(request.Status))
            .Where(r => r.RegulatoryBody!.Contains(request.RegulatoryBody!), !string.IsNullOrEmpty(request.RegulatoryBody))
            .Where(r => r.PeriodStartDate >= request.PeriodStartDate, request.PeriodStartDate.HasValue)
            .Where(r => r.PeriodEndDate <= request.PeriodEndDate, request.PeriodEndDate.HasValue);
}
