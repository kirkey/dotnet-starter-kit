namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;

/// <summary>
/// Response DTO for item supplier search results.
/// </summary>
/// <remarks>
/// Contains item supplier relationship information for search and listing operations.
/// Used in procurement, sourcing, and supplier management operations.
/// 
/// Default values:
/// - ItemId: required item reference
/// - SupplierId: required supplier reference
/// - IsPreferred: false (indicates if this is the preferred supplier)
/// - LeadTimeDays: 0 (supplier lead time in days)
/// - MinimumOrderQuantity: 0 (minimum order quantity)
/// </remarks>
public sealed record ItemSupplierResponse
{
    /// <summary>
    /// Unique item supplier relationship identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Associated item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; init; }

    /// <summary>
    /// Item name for display purposes.
    /// </summary>
    public string ItemName { get; init; } = default!;

    /// <summary>
    /// Item SKU for reference.
    /// </summary>
    public string ItemSku { get; init; } = default!;

    /// <summary>
    /// Associated supplier identifier.
    /// </summary>
    public DefaultIdType SupplierId { get; init; }

    /// <summary>
    /// Supplier name for display purposes.
    /// </summary>
    public string SupplierName { get; init; } = default!;

    /// <summary>
    /// Supplier code for reference.
    /// </summary>
    public string? SupplierCode { get; init; }

    /// <summary>
    /// Supplier's part number for this item.
    /// </summary>
    public string? SupplierPartNumber { get; init; }

    /// <summary>
    /// Whether this is the preferred supplier for the item.
    /// </summary>
    public bool IsPreferred { get; init; }

    /// <summary>
    /// Supplier lead time in days.
    /// </summary>
    public int LeadTimeDays { get; init; }

    /// <summary>
    /// Minimum order quantity from this supplier.
    /// </summary>
    public decimal MinimumOrderQuantity { get; init; }

    /// <summary>
    /// Current unit cost from this supplier.
    /// </summary>
    public decimal? UnitCost { get; init; }

    /// <summary>
    /// Currency for pricing.
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Date when pricing was last updated.
    /// </summary>
    public DateTime? LastPriceUpdate { get; init; }

    /// <summary>
    /// Whether this supplier relationship is active.
    /// </summary>
    public bool IsActive { get; init; } = true;
}
