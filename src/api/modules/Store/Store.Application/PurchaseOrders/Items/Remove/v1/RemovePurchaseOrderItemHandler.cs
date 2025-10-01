using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;

public sealed class RemovePurchaseOrderItemHandler(
    ILogger<RemovePurchaseOrderItemHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository,
    [FromKeyedServices("store:purchase-order-items")] IRepository<PurchaseOrderItem> itemRepository,
    [FromKeyedServices("store:purchase-order-items")] IReadRepository<PurchaseOrderItem> itemReadRepository)
    : IRequestHandler<RemovePurchaseOrderItemCommand>
{
    public async Task Handle(RemovePurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Ensure purchase order exists
        var po = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.PurchaseOrderId);

        // Find and remove the item
        var item = (await itemReadRepository.ListAsync(cancellationToken).ConfigureAwait(false))
            .FirstOrDefault(i => i.PurchaseOrderId == request.PurchaseOrderId && i.GroceryItemId == request.GroceryItemId);
        
        if (item != null)
        {
            await itemRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);
            
            // Recalculate purchase order totals
            var allItems = (await itemReadRepository.ListAsync(cancellationToken).ConfigureAwait(false))
                .Where(i => i.PurchaseOrderId == request.PurchaseOrderId)
                .ToList();
            
            var totalAmount = allItems.Sum(i => i.TotalPrice);
            po.UpdateTotals(totalAmount);

            await purchaseOrderRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("Removed item from purchase order {PurchaseOrderId}", po.Id);
    }
}

