namespace FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;

/// <summary>
/// Request to search stock levels with filters and pagination.
/// </summary>
public class SearchStockLevelsCommand : PaginationFilter, IRequest<PagedList<StockLevelResponse>>
{
    /// <summary>
    /// Filter by item ID.
    /// </summary>
    public DefaultIdType? ItemId { get; init; }

    /// <summary>
    /// Filter by warehouse ID.
    /// </summary>
    public DefaultIdType? WarehouseId { get; init; }

    /// <summary>
    /// Filter by warehouse location ID.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; init; }

    /// <summary>
    /// Filter by bin ID.
    /// </summary>
    public DefaultIdType? BinId { get; init; }

    /// <summary>
    /// Filter by lot number ID.
    /// </summary>
    public DefaultIdType? LotNumberId { get; init; }

    /// <summary>
    /// Filter by serial number ID.
    /// </summary>
    public DefaultIdType? SerialNumberId { get; init; }

    /// <summary>
    /// Filter by minimum quantity on hand.
    /// </summary>
    public int? MinQuantityOnHand { get; init; }

    /// <summary>
    /// Filter by maximum quantity on hand.
    /// </summary>
    public int? MaxQuantityOnHand { get; init; }

    /// <summary>
    /// Filter by minimum available quantity.
    /// </summary>
    public int? MinQuantityAvailable { get; init; }

    /// <summary>
    /// Filter by having reserved quantities.
    /// </summary>
    public bool? HasReservedQuantity { get; init; }

    /// <summary>
    /// Filter by having allocated quantities.
    /// </summary>
    public bool? HasAllocatedQuantity { get; init; }
}
