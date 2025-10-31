using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Specs;

namespace Accounting.Application.RegulatoryReports.Search.v1;

public sealed class SearchRegulatoryReportsRequestHandler(
    [FromKeyedServices("accounting:regulatoryreports")] IReadRepository<RegulatoryReport> repository)
    : IRequestHandler<SearchRegulatoryReportsRequest, PagedList<RegulatoryReportResponse>>
{
    public async Task<PagedList<RegulatoryReportResponse>> Handle(SearchRegulatoryReportsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRegulatoryReportsSpec(request);
        var allReports = await repository.ListAsync(spec, cancellationToken);

        var pagedReports = allReports
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var totalCount = allReports.Count;

        return new PagedList<RegulatoryReportResponse>(
            pagedReports.Adapt<List<RegulatoryReportResponse>>(),
            request.PageNumber,
            request.PageSize,
            totalCount
        );
    }
}
