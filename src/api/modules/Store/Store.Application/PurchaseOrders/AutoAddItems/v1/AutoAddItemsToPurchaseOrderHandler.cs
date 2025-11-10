using FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;
using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;

/// <summary>
/// Handler to automatically add items needing reorder to a purchase order.
/// </summary>
public class AutoAddItemsToPurchaseOrderHandler(
    IRepository<PurchaseOrder> purchaseOrderRepository,
    ISender mediator,
    ILogger<AutoAddItemsToPurchaseOrderHandler> logger)
    : IRequestHandler<AutoAddItemsToPurchaseOrderCommand, AutoAddItemsToPurchaseOrderResponse>
{
    public async Task<AutoAddItemsToPurchaseOrderResponse> Handle(AutoAddItemsToPurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        // Get the purchase order
        var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(command.PurchaseOrderId, cancellationToken);
        if (purchaseOrder is null)
        {
            throw new PurchaseOrderNotFoundException(command.PurchaseOrderId);
        }

        // Get items needing reorder for this supplier
        var itemsNeedingReorder = await mediator.Send(new GetItemsNeedingReorderRequest
        {
            SupplierId = purchaseOrder.SupplierId,
            WarehouseId = command.WarehouseId
        }, cancellationToken);

        if (itemsNeedingReorder.Count == 0)
        {
            logger.LogInformation("No items needing reorder found for supplier {SupplierId}", purchaseOrder.SupplierId);
            return new AutoAddItemsToPurchaseOrderResponse
            {
                PurchaseOrderId = command.PurchaseOrderId,
                ItemsAdded = 0,
                ItemsSkipped = 0,
                TotalEstimatedCost = 0
            };
        }

        var addedItems = new List<AddedItemInfo>();
        var skippedItems = new List<SkippedItemInfo>();
        decimal totalCost = 0;

        // Get existing items on the order to avoid duplicates
        var existingItemIds = purchaseOrder.Items.Select(i => i.ItemId).ToHashSet();

        foreach (var item in itemsNeedingReorder)
        {
            // Skip if item already on order
            if (existingItemIds.Contains(item.Id))
            {
                skippedItems.Add(new SkippedItemInfo
                {
                    ItemId = item.Id,
                    Sku = item.Sku,
                    Name = item.Name,
                    Reason = "Item already on this purchase order"
                });
                continue;
            }

            try
            {
                // Determine quantity to order
                var quantity = command.UseSuggestedQuantities
                    ? item.SuggestedOrderQuantity
                    : item.ReorderQuantity;

                if (quantity <= 0)
                {
                    skippedItems.Add(new SkippedItemInfo
                    {
                        ItemId = item.Id,
                        Sku = item.Sku,
                        Name = item.Name,
                        Reason = "Calculated quantity is zero or negative"
                    });
                    continue;
                }

                // Add item to purchase order
                purchaseOrder.AddItem(item.Id, quantity, item.Cost);

                var itemTotal = quantity * item.Cost;
                totalCost += itemTotal;

                addedItems.Add(new AddedItemInfo
                {
                    ItemId = item.Id,
                    Sku = item.Sku,
                    Name = item.Name,
                    Quantity = quantity,
                    UnitPrice = item.Cost,
                    TotalCost = itemTotal
                });

                logger.LogInformation("Added item {Sku} ({Name}) to purchase order {POId}: Qty={Qty}, Cost={Cost}",
                    item.Sku, item.Name, command.PurchaseOrderId, quantity, item.Cost);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to add item {Sku} to purchase order {POId}", item.Sku, command.PurchaseOrderId);
                skippedItems.Add(new SkippedItemInfo
                {
                    ItemId = item.Id,
                    Sku = item.Sku,
                    Name = item.Name,
                    Reason = $"Error: {ex.Message}"
                });
            }
        }

        // Save the purchase order with new items
        await purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
        await purchaseOrderRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Auto-added {AddedCount} items to purchase order {POId}, skipped {SkippedCount} items",
            addedItems.Count, command.PurchaseOrderId, skippedItems.Count);

        return new AutoAddItemsToPurchaseOrderResponse
        {
            PurchaseOrderId = command.PurchaseOrderId,
            ItemsAdded = addedItems.Count,
            ItemsSkipped = skippedItems.Count,
            TotalEstimatedCost = totalCost,
            AddedItems = addedItems,
            SkippedItems = skippedItems
        };
    }
}
