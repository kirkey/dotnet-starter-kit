using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

public sealed class GetPurchaseOrderListHandler(
    ILogger<GetPurchaseOrderListHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<SearchPurchaseOrdersCommand, PagedList<GetPurchaseOrderListResponse>>
{
    public async Task<PagedList<GetPurchaseOrderListResponse>> Handle(SearchPurchaseOrdersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPurchaseOrdersSpecs(request);
        var paged = await repository.PaginatedListAsync(spec, new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize }, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Search complete: retrieved {Count} purchase orders", paged.TotalCount);
        return paged;
    }
}
