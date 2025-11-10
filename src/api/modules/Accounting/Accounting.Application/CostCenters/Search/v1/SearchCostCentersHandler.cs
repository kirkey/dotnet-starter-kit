using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Search.v1;

/// <summary>
/// Handler for searching cost centers with filters and pagination.
/// </summary>
public sealed class SearchCostCentersHandler(
    [FromKeyedServices("accounting:costCenters")] IReadRepository<CostCenter> repository)
    : IRequestHandler<SearchCostCentersRequest, PagedList<CostCenterResponse>>
{
    public async Task<PagedList<CostCenterResponse>> Handle(SearchCostCentersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCostCentersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CostCenterResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
