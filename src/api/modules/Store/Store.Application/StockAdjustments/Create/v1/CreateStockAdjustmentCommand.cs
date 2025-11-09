namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;

/// <summary>
/// Command to create a new stock adjustment.
/// </summary>
public record CreateStockAdjustmentCommand : IRequest<CreateStockAdjustmentResponse>
{
    /// <summary>
    /// Gets or sets the adjustment name.
    /// </summary>
    [DefaultValue("Stock Count Adjustment")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the adjustment description.
    /// </summary>
    [DefaultValue("Inventory count adjustment")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the adjustment number.
    /// </summary>
    [DefaultValue("ADJ001")]
    public string AdjustmentNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; init; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    public DefaultIdType WarehouseId { get; init; }

    /// <summary>
    /// Gets or sets the warehouse location identifier.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; init; }

    /// <summary>
    /// Gets or sets the adjustment date.
    /// </summary>
    [DefaultValue("2024-01-01")]
    public DateTime AdjustmentDate { get; init; }

    /// <summary>
    /// Gets or sets the adjustment type.
    /// </summary>
    [DefaultValue("Physical Count")]
    public string AdjustmentType { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    [DefaultValue("Pending")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the reason for adjustment.
    /// </summary>
    [DefaultValue("Monthly inventory count")]
    public string Reason { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity before adjustment.
    /// </summary>
    [DefaultValue(0)]
    public int QuantityBefore { get; init; }

    /// <summary>
    /// Gets or sets the adjustment quantity.
    /// </summary>
    [DefaultValue(0)]
    public int AdjustmentQuantity { get; init; }

    /// <summary>
    /// Gets or sets the unit cost.
    /// </summary>
    [DefaultValue(0.0)]
    public decimal UnitCost { get; init; }

    /// <summary>
    /// Gets or sets the reference.
    /// </summary>
    [DefaultValue(null)]
    public string? Reference { get; init; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; init; }

    /// <summary>
    /// Gets or sets who adjusted.
    /// </summary>
    [DefaultValue(null)]
    public string? AdjustedBy { get; init; }

    /// <summary>
    /// Gets or sets the batch number.
    /// </summary>
    [DefaultValue(null)]
    public string? BatchNumber { get; init; }

    /// <summary>
    /// Gets or sets the expiry date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpiryDate { get; init; }
}
