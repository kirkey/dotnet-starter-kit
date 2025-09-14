using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;

public sealed class ReceivePurchaseOrderItemQuantityHandler(
    ILogger<ReceivePurchaseOrderItemQuantityHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<ReceivePurchaseOrderItemQuantityCommand>
{
    public async Task Handle(ReceivePurchaseOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.FirstOrDefaultAsync(new Specs.PurchaseOrderByItemIdSpec(request.PurchaseOrderItemId), cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderItemNotFoundException(request.PurchaseOrderItemId);

        var updated = po.ReceiveItemQuantity(request.PurchaseOrderItemId, request.ReceivedQuantity);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Received quantity updated for purchase order {PurchaseOrderId}", po.Id);
    }
}
