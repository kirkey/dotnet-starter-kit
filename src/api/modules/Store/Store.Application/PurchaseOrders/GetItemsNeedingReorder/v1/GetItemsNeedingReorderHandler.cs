namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;

/// <summary>
/// Handler to get items that need reordering for a specific supplier.
/// </summary>
public class GetItemsNeedingReorderHandler(
    IReadRepository<Item> itemRepository,
    IReadRepository<StockLevel> stockLevelRepository,
    IReadRepository<Supplier> supplierRepository,
    ILogger<GetItemsNeedingReorderHandler> logger)
    : IRequestHandler<GetItemsNeedingReorderRequest, List<ItemNeedingReorderResponse>>
{
    public async Task<List<ItemNeedingReorderResponse>> Handle(GetItemsNeedingReorderRequest request, CancellationToken cancellationToken)
    {
        // Get items for the specified supplier
        var spec = new GetItemsBySupplierSpec(request.SupplierId);
        var items = await itemRepository.ListAsync(spec, cancellationToken);

        if (items.Count == 0)
        {
            logger.LogInformation("No active items found for supplier {SupplierId}", request.SupplierId);
            return new List<ItemNeedingReorderResponse>();
        }

        // Get supplier information
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);
        var supplierName = supplier?.Name ?? "Unknown";

        var itemsNeedingReorder = new List<ItemNeedingReorderResponse>();

        foreach (var item in items)
        {
            // Get current stock for this item
            int currentStock;
            if (request.WarehouseId.HasValue)
            {
                var warehouseStockSpec = new GetStockByItemAndWarehouseSpec(item.Id, request.WarehouseId.Value);
                var warehouseStockLevels = await stockLevelRepository.ListAsync(warehouseStockSpec, cancellationToken);
                currentStock = warehouseStockLevels.Sum(sl => sl.QuantityOnHand);
            }
            else
            {
                var allStockSpec = new GetStockByItemSpec(item.Id);
                var allStockLevels = await stockLevelRepository.ListAsync(allStockSpec, cancellationToken);
                currentStock = allStockLevels.Sum(sl => sl.QuantityOnHand);
            }

            // Check if item needs reordering
            if (currentStock <= item.ReorderPoint)
            {
                var shortage = Math.Max(0, item.ReorderPoint - currentStock);
                var suggestedQty = CalculateSuggestedOrderQuantity(
                    currentStock,
                    item.ReorderPoint,
                    item.ReorderQuantity,
                    item.MinimumStock,
                    item.MaximumStock);

                itemsNeedingReorder.Add(new ItemNeedingReorderResponse
                {
                    Id = item.Id,
                    Sku = item.Sku,
                    Name = item.Name,
                    Description = item.Description,
                    CurrentStock = currentStock,
                    ReorderPoint = item.ReorderPoint,
                    ReorderQuantity = item.ReorderQuantity,
                    Cost = item.Cost,
                    SupplierId = item.SupplierId,
                    SupplierName = supplierName,
                    LeadTimeDays = item.LeadTimeDays,
                    MinimumStock = item.MinimumStock,
                    MaximumStock = item.MaximumStock,
                    ShortageQuantity = shortage,
                    SuggestedOrderQuantity = suggestedQty,
                    EstimatedCost = suggestedQty * item.Cost
                });
            }
        }

        logger.LogInformation("Found {Count} items needing reorder for supplier {SupplierId}",
            itemsNeedingReorder.Count, request.SupplierId);

        return itemsNeedingReorder
            .OrderBy(x => x.CurrentStock) // Order by most critical (lowest stock) first
            .ThenBy(x => x.Sku)
            .ToList();
    }

    /// <summary>
    /// Calculates the suggested order quantity based on current stock and reorder parameters.
    /// </summary>
    private static int CalculateSuggestedOrderQuantity(int currentStock, int reorderPoint, int reorderQuantity, int minimumStock, int maximumStock)
    {
        // If we're below minimum stock, order enough to reach the reorder quantity target
        if (currentStock < minimumStock)
        {
            var targetStock = Math.Min(reorderPoint + reorderQuantity, maximumStock);
            return Math.Max(targetStock - currentStock, reorderQuantity);
        }

        // If we're at or below reorder point but above minimum, order the standard reorder quantity
        if (currentStock <= reorderPoint)
        {
            // Make sure we don't exceed maximum stock
            var spaceAvailable = maximumStock - currentStock;
            return Math.Min(reorderQuantity, spaceAvailable);
        }

        // Default to reorder quantity
        return reorderQuantity;
    }
}

// Specification to get items by supplier
internal class GetItemsBySupplierSpec : Specification<Item>
{
    public GetItemsBySupplierSpec(DefaultIdType supplierId)
    {
        Query.Where(i => i.SupplierId == supplierId);
    }
}

// Specification to get stock levels by item
internal class GetStockByItemSpec : Specification<StockLevel>
{
    public GetStockByItemSpec(DefaultIdType itemId)
    {
        Query.Where(sl => sl.ItemId == itemId);
    }
}

// Specification to get stock levels by item and warehouse
internal class GetStockByItemAndWarehouseSpec : Specification<StockLevel>
{
    public GetStockByItemAndWarehouseSpec(DefaultIdType itemId, DefaultIdType warehouseId)
    {
        Query.Where(sl => sl.ItemId == itemId && sl.WarehouseId == warehouseId);
    }
}

