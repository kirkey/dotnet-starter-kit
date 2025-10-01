using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;

public sealed class UpdatePurchaseOrderItemQuantityHandler(
    ILogger<UpdatePurchaseOrderItemQuantityHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository,
    [FromKeyedServices("store:purchase-order-items")] IRepository<PurchaseOrderItem> itemRepository,
    [FromKeyedServices("store:purchase-order-items")] IReadRepository<PurchaseOrderItem> itemReadRepository)
    : IRequestHandler<UpdatePurchaseOrderItemQuantityCommand>
{
    public async Task Handle(UpdatePurchaseOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Find the item
        var item = await itemRepository.GetByIdAsync(request.PurchaseOrderItemId, cancellationToken).ConfigureAwait(false);
        _ = item ?? throw new PurchaseOrderItemNotFoundException(request.PurchaseOrderItemId);

        // Ensure purchase order exists
        var po = await purchaseOrderRepository.GetByIdAsync(item.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(item.PurchaseOrderId);

        // Update the item quantity
        item.UpdateQuantity(request.Quantity);
        await itemRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);

        // Recalculate purchase order totals
        var allItems = (await itemReadRepository.ListAsync(cancellationToken).ConfigureAwait(false))
            .Where(i => i.PurchaseOrderId == item.PurchaseOrderId)
            .ToList();
        
        var totalAmount = allItems.Sum(i => i.TotalPrice);
        po.UpdateTotals(totalAmount);

        await purchaseOrderRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated item quantity for purchase order {PurchaseOrderId}", po.Id);
    }
}
