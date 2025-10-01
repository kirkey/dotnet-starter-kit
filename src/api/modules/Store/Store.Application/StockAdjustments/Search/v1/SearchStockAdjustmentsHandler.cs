using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Specs;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1;

public sealed class SearchStockAdjustmentsHandler(
    [FromKeyedServices("store:stock-adjustments")] IReadRepository<StockAdjustment> repository)
    : IRequestHandler<SearchStockAdjustmentsCommand, PagedList<StockAdjustmentResponse>>
{
    public async Task<PagedList<StockAdjustmentResponse>> Handle(SearchStockAdjustmentsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchStockAdjustmentsSpecs(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<StockAdjustmentResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
