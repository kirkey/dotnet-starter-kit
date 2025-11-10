namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;

/// <summary>
/// Response after auto-adding items to a purchase order.
/// </summary>
public record AutoAddItemsToPurchaseOrderResponse
{
    /// <summary>
    /// Purchase order ID.
    /// </summary>
    public DefaultIdType PurchaseOrderId { get; init; }

    /// <summary>
    /// Number of items added.
    /// </summary>
    public int ItemsAdded { get; init; }

    /// <summary>
    /// Number of items skipped (already on order or other reason).
    /// </summary>
    public int ItemsSkipped { get; init; }

    /// <summary>
    /// Total estimated cost of items added.
    /// </summary>
    public decimal TotalEstimatedCost { get; init; }

    /// <summary>
    /// List of items that were added.
    /// </summary>
    public List<AddedItemInfo> AddedItems { get; init; } = new();

    /// <summary>
    /// List of items that were skipped with reasons.
    /// </summary>
    public List<SkippedItemInfo> SkippedItems { get; init; } = new();
}

/// <summary>
/// Information about an item that was added to the purchase order.
/// </summary>
public record AddedItemInfo
{
    public DefaultIdType ItemId { get; init; }
    public string Sku { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalCost { get; init; }
}

/// <summary>
/// Information about an item that was skipped.
/// </summary>
public record SkippedItemInfo
{
    public DefaultIdType ItemId { get; init; }
    public string Sku { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

