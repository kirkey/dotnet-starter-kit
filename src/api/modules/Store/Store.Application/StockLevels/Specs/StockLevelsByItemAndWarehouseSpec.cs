namespace FSH.Starter.WebApi.Store.Application.StockLevels.Specs;

/// <summary>
/// Specification to find stock levels by item ID and warehouse ID.
/// </summary>
public class StockLevelsByItemAndWarehouseSpec : Specification<StockLevel>
{
    public StockLevelsByItemAndWarehouseSpec(DefaultIdType itemId, DefaultIdType warehouseId)
    {
        Query
            .Where(sl => sl.ItemId == itemId && sl.WarehouseId == warehouseId);
    }
}

