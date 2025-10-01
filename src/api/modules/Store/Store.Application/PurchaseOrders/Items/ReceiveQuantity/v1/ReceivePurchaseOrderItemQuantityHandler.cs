using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;

public sealed class ReceivePurchaseOrderItemQuantityHandler(
    ILogger<ReceivePurchaseOrderItemQuantityHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository,
    [FromKeyedServices("store:purchase-order-items")] IRepository<PurchaseOrderItem> itemRepository)
    : IRequestHandler<ReceivePurchaseOrderItemQuantityCommand>
{
    public async Task Handle(ReceivePurchaseOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Find the item
        var item = await itemRepository.GetByIdAsync(request.PurchaseOrderItemId, cancellationToken).ConfigureAwait(false);
        _ = item ?? throw new PurchaseOrderItemNotFoundException(request.PurchaseOrderItemId);

        // Ensure purchase order exists
        var po = await purchaseOrderRepository.GetByIdAsync(item.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(item.PurchaseOrderId);

        // Update the received quantity
        item.ReceiveQuantity(request.ReceivedQuantity);
        await itemRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

        // Note: Receiving quantity doesn't affect total price, so no need to recalculate totals

        await purchaseOrderRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Received quantity updated for purchase order {PurchaseOrderId}", po.Id);
    }
}
