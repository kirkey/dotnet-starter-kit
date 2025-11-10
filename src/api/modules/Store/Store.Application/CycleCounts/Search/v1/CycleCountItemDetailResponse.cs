namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

/// <summary>
/// Detailed response for cycle count items including full item information.
/// Used for mobile counting interface with all necessary item details.
/// </summary>
public sealed record CycleCountItemDetailResponse
{
    /// <summary>
    /// The cycle count item identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The cycle count identifier.
    /// </summary>
    public DefaultIdType CycleCountId { get; init; }

    /// <summary>
    /// The item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; init; }

    /// <summary>
    /// The item SKU.
    /// </summary>
    public string? ItemSku { get; init; }

    /// <summary>
    /// The item barcode.
    /// </summary>
    public string? ItemBarcode { get; init; }

    /// <summary>
    /// The item name.
    /// </summary>
    public string? ItemName { get; init; }

    /// <summary>
    /// The item description.
    /// </summary>
    public string? ItemDescription { get; init; }

    /// <summary>
    /// The warehouse location name or code.
    /// </summary>
    public string? LocationName { get; init; }

    /// <summary>
    /// System quantity before counting (expected quantity).
    /// </summary>
    public decimal ExpectedQuantity { get; init; }

    /// <summary>
    /// Actual counted quantity.
    /// </summary>
    public decimal ActualQuantity { get; init; }

    /// <summary>
    /// Variance between actual and expected.
    /// </summary>
    public decimal VarianceAmount { get; init; }

    /// <summary>
    /// Variance percentage.
    /// </summary>
    public decimal VariancePercentage { get; init; }

    /// <summary>
    /// Whether the item has been counted.
    /// </summary>
    public bool IsCounted { get; init; }

    /// <summary>
    /// Date when the item was counted.
    /// </summary>
    public DateTime? CountDate { get; init; }

    /// <summary>
    /// User who counted the item.
    /// </summary>
    public string? CountedBy { get; init; }

    /// <summary>
    /// Whether the item requires recounting.
    /// </summary>
    public bool RequiresRecount { get; init; }

    /// <summary>
    /// Reason for recounting.
    /// </summary>
    public string? RecountReason { get; init; }

    /// <summary>
    /// Additional notes about the count.
    /// </summary>
    public string? Notes { get; init; }
}

