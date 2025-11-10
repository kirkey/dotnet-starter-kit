namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

/// <summary>
/// Request to search cycle count items within a specific cycle count.
/// Used for mobile counting interface to load items and search by barcode/SKU.
/// </summary>
public sealed class SearchCycleCountItemsRequest : PaginationFilter, IRequest<PagedList<CycleCountItemDetailResponse>>
{
    /// <summary>
    /// The cycle count identifier to search items for.
    /// </summary>
    public DefaultIdType? CycleCountId { get; init; }

    /// <summary>
    /// Filter by item SKU.
    /// </summary>
    public string? ItemSku { get; init; }

    /// <summary>
    /// Filter by item barcode.
    /// </summary>
    public string? ItemBarcode { get; init; }

    /// <summary>
    /// Filter by item name (partial match).
    /// </summary>
    public string? ItemName { get; init; }

    /// <summary>
    /// Filter by whether the item has been counted.
    /// </summary>
    public bool? IsCounted { get; init; }

    /// <summary>
    /// Filter by whether the item has variance.
    /// </summary>
    public bool? HasVariance { get; init; }

    /// <summary>
    /// Filter by whether the item requires recount.
    /// </summary>
    public bool? RequiresRecount { get; init; }
}

