namespace FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;

/// <summary>
/// Response DTO for stock level search results.
/// </summary>
/// <remarks>
/// Contains stock level information for inventory tracking and management operations.
/// Used in inventory reports, stock alerts, and replenishment planning.
/// 
/// Default values:
/// - ItemId: required item reference
/// - WarehouseId: required warehouse reference
/// - QuantityOnHand: current available quantity
/// - QuantityReserved: quantity allocated to orders
/// - QuantityAvailable: on hand minus reserved
/// - ReorderPoint: minimum stock level before reorder
/// </remarks>
public sealed record StockLevelResponse
{
    /// <summary>
    /// Unique stock level identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Name for display purposes.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional notes.
    /// </summary>
    public string? Notes { get; init; }

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
    /// Associated warehouse identifier.
    /// </summary>
    public DefaultIdType WarehouseId { get; init; }

    /// <summary>
    /// Warehouse name for display purposes.
    /// </summary>
    public string WarehouseName { get; init; } = default!;

    /// <summary>
    /// Warehouse code for reference.
    /// </summary>
    public string? WarehouseCode { get; init; }

    /// <summary>
    /// Current quantity on hand.
    /// </summary>
    public decimal QuantityOnHand { get; init; }

    /// <summary>
    /// Quantity reserved for orders.
    /// </summary>
    public decimal QuantityReserved { get; init; }

    /// <summary>
    /// Available quantity (on hand minus reserved).
    /// </summary>
    public decimal QuantityAvailable => QuantityOnHand - QuantityReserved;

    /// <summary>
    /// Quantity on order from suppliers.
    /// </summary>
    public decimal QuantityOnOrder { get; init; }

    /// <summary>
    /// Reorder point for replenishment.
    /// </summary>
    public decimal ReorderPoint { get; init; }

    /// <summary>
    /// Maximum stock level.
    /// </summary>
    public decimal? MaximumLevel { get; init; }

    /// <summary>
    /// Unit of measure for quantities.
    /// </summary>
    public string? UnitOfMeasure { get; init; }

    /// <summary>
    /// Date when stock level was last updated.
    /// </summary>
    public DateTime LastUpdated { get; init; }

    /// <summary>
    /// Whether stock is below reorder point.
    /// </summary>
    public bool IsBelowReorderPoint => QuantityOnHand <= ReorderPoint;
}
