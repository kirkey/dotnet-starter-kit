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
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Search complete: retrieved {Count} purchase orders", request.PageNumber, request.PageSize, totalCount);
        return new PagedList<GetPurchaseOrderListResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
