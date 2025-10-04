namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

/// <summary>
/// Command to update an existing stock adjustment.
/// </summary>
public record UpdateStockAdjustmentCommand : IRequest<UpdateStockAdjustmentResponse>
{
    /// <summary>
    /// Gets or sets the stock adjustment identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the stock adjustment name.
    /// </summary>
    [DefaultValue("Stock Adjustment ADJ-2025-001")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stock adjustment description.
    /// </summary>
    [DefaultValue("Stock adjustment description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the adjustment reference number.
    /// </summary>
    [DefaultValue("ADJ-2025-001")]
    public string AdjustmentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    public DefaultIdType WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse location identifier.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; set; }

    /// <summary>
    /// Gets or sets the date of the adjustment.
    /// </summary>
    [DefaultValue("2025-10-04")]
    public DateTime? AdjustmentDate { get; set; }

    /// <summary>
    /// Gets or sets the type of adjustment.
    /// </summary>
    [DefaultValue("Increase")]
    public string AdjustmentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reason for the adjustment.
    /// </summary>
    [DefaultValue("Physical count correction")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity before adjustment.
    /// </summary>
    [DefaultValue(100)]
    public int QuantityBefore { get; set; }

    /// <summary>
    /// Gets or sets the quantity adjusted.
    /// </summary>
    [DefaultValue(10)]
    public int QuantityAdjusted { get; set; }

    /// <summary>
    /// Gets or sets the quantity after adjustment.
    /// </summary>
    [DefaultValue(110)]
    public int QuantityAfter { get; set; }

    /// <summary>
    /// Gets or sets the unit cost.
    /// </summary>
    [DefaultValue(10.00)]
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Gets or sets the total cost impact.
    /// </summary>
    [DefaultValue(100.00)]
    public decimal TotalCostImpact { get; set; }

    /// <summary>
    /// Gets or sets the reference number.
    /// </summary>
    [DefaultValue(null)]
    public string? Reference { get; set; }

    /// <summary>
    /// Gets or sets who adjusted the stock.
    /// </summary>
    [DefaultValue(null)]
    public string? AdjustedBy { get; set; }

    /// <summary>
    /// Gets or sets the batch number.
    /// </summary>
    [DefaultValue(null)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Gets or sets the expiry date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

