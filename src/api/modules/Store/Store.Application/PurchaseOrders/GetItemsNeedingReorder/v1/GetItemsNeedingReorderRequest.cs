namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;

/// <summary>
/// Request to get items that need reordering for a specific supplier.
/// </summary>
public record GetItemsNeedingReorderRequest : IRequest<List<ItemNeedingReorderResponse>>
{
    /// <summary>
    /// Supplier ID to filter items by.
    /// </summary>
    public DefaultIdType SupplierId { get; init; }

    /// <summary>
    /// Optional warehouse ID to check stock levels at specific location.
    /// If null, checks total stock across all warehouses.
    /// </summary>
    public DefaultIdType? WarehouseId { get; init; }
}

