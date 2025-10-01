namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Get.v1;

public sealed class GetPurchaseOrderItemsHandler(
    ILogger<GetPurchaseOrderItemsHandler> logger,
    [FromKeyedServices("store:purchase-order-items")] IReadRepository<PurchaseOrderItem> itemRepository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryItemRepository)
    : IRequestHandler<GetPurchaseOrderItemsQuery, List<PurchaseOrderItemResponse>>
{
    public async Task<List<PurchaseOrderItemResponse>> Handle(GetPurchaseOrderItemsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Getting items for purchase order {PurchaseOrderId}", request.PurchaseOrderId);

        var items = await itemRepository.ListAsync(cancellationToken).ConfigureAwait(false);
        var purchaseOrderItems = items.Where(i => i.PurchaseOrderId == request.PurchaseOrderId).ToList();

        // Get all grocery items for the purchase order items
        var groceryItemIds = purchaseOrderItems.Select(i => i.GroceryItemId).Distinct().ToList();
        var groceryItems = await groceryItemRepository.ListAsync(cancellationToken).ConfigureAwait(false);
        var groceryItemsDict = groceryItems
            .Where(g => groceryItemIds.Contains(g.Id))
            .ToDictionary(g => g.Id);

        var response = purchaseOrderItems.Select(item => new PurchaseOrderItemResponse(
            item.Id,
            item.PurchaseOrderId,
            item.GroceryItemId,
            groceryItemsDict.TryGetValue(item.GroceryItemId, out var groceryItem) ? groceryItem.Name : "Unknown",
            groceryItemsDict.TryGetValue(item.GroceryItemId, out var gi) ? gi.Sku : "Unknown",
            item.Quantity,
            item.UnitPrice,
            item.DiscountAmount,
            item.TotalPrice,
            item.ReceivedQuantity,
            item.Notes
        )).ToList();

        logger.LogInformation("Retrieved {Count} items for purchase order {PurchaseOrderId}", response.Count, request.PurchaseOrderId);

        return response;
    }
}
