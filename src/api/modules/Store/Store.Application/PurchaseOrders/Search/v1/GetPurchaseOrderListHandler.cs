using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;
using Store.Domain;

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

        var mappedItems = items.Select(po => new GetPurchaseOrderListResponse(
            po.Id,
            po.OrderNumber,
            po.SupplierId,
            po.OrderDate,
            po.Status,
            po.TotalAmount)).ToList();

        logger.LogInformation("Search complete: retrieved {Count} purchase orders", totalCount);
        return new PagedList<GetPurchaseOrderListResponse>(mappedItems, request.PageNumber, request.PageSize, totalCount);
    }
}
