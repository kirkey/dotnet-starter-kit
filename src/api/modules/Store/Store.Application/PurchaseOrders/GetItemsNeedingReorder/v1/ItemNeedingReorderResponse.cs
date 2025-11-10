namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;

/// <summary>
/// Response containing item details that need reordering.
/// </summary>
public record ItemNeedingReorderResponse
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Item SKU.
    /// </summary>
    public string Sku { get; init; } = default!;

    /// <summary>
    /// Item name.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Item description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Current total stock on hand across all locations (or specific warehouse if filtered).
    /// </summary>
    public int CurrentStock { get; init; }

    /// <summary>
    /// Reorder point threshold.
    /// </summary>
    public int ReorderPoint { get; init; }

    /// <summary>
    /// Recommended quantity to order.
    /// </summary>
    public int ReorderQuantity { get; init; }

    /// <summary>
    /// Supplier cost per unit.
    /// </summary>
    public decimal Cost { get; init; }

    /// <summary>
    /// Supplier ID.
    /// </summary>
    public DefaultIdType SupplierId { get; init; }

    /// <summary>
    /// Supplier name.
    /// </summary>
    public string SupplierName { get; init; } = default!;

    /// <summary>
    /// Lead time in days from supplier.
    /// </summary>
    public int LeadTimeDays { get; init; }

    /// <summary>
    /// Minimum stock level (safety stock).
    /// </summary>
    public int MinimumStock { get; init; }

    /// <summary>
    /// Maximum stock level.
    /// </summary>
    public int MaximumStock { get; init; }

    /// <summary>
    /// Calculated shortage quantity (ReorderPoint - CurrentStock).
    /// </summary>
    public int ShortageQuantity { get; init; }

    /// <summary>
    /// Suggested order quantity based on shortage and reorder quantity.
    /// </summary>
    public int SuggestedOrderQuantity { get; init; }

    /// <summary>
    /// Estimated total cost for suggested order quantity.
    /// </summary>
    public decimal EstimatedCost { get; init; }
}

