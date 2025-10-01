using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

public sealed class AddPurchaseOrderItemHandler(
    ILogger<AddPurchaseOrderItemHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository,
    [FromKeyedServices("store:purchase-order-items")] IRepository<PurchaseOrderItem> itemRepository,
    [FromKeyedServices("store:purchase-order-items")] IReadRepository<PurchaseOrderItem> itemReadRepository)
    : IRequestHandler<AddPurchaseOrderItemCommand, AddPurchaseOrderItemResponse>
{
    public async Task<AddPurchaseOrderItemResponse> Handle(AddPurchaseOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Ensure purchase order exists and is modifiable
        var po = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.PurchaseOrderId);

        // Check if item already exists for this purchase order
        var existingItem = (await itemReadRepository.ListAsync(cancellationToken).ConfigureAwait(false))
            .FirstOrDefault(i => i.PurchaseOrderId == request.PurchaseOrderId && i.GroceryItemId == request.GroceryItemId);

        if (existingItem != null)
        {
            // Update existing item quantity
            existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
            await itemRepository.UpdateAsync(existingItem, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            // Create new item
            var newItem = PurchaseOrderItem.Create(
                request.PurchaseOrderId,
                request.GroceryItemId,
                request.Quantity,
                request.UnitPrice,
                request.Discount);
            await itemRepository.AddAsync(newItem, cancellationToken).ConfigureAwait(false);
        }

        // Recalculate purchase order totals
        var allItems = (await itemReadRepository.ListAsync(cancellationToken).ConfigureAwait(false))
            .Where(i => i.PurchaseOrderId == request.PurchaseOrderId)
            .ToList();
        
        var totalAmount = allItems.Sum(i => i.TotalPrice);
        po.UpdateTotals(totalAmount);

        await purchaseOrderRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Added item to purchase order {PurchaseOrderId}", po.Id);
        return new AddPurchaseOrderItemResponse(po.Id);
    }
}

