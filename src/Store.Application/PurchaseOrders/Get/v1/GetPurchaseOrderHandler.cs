using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;

public sealed class GetPurchaseOrderHandler(
    ILogger<GetPurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<GetPurchaseOrderQuery, GetPurchaseOrderResponse>
{
    public async Task<GetPurchaseOrderResponse> Handle(GetPurchaseOrderQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetPurchaseOrderSpecification(request.Id);
        var po = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.Id);

        return po.Adapt<GetPurchaseOrderResponse>();
    }
}
