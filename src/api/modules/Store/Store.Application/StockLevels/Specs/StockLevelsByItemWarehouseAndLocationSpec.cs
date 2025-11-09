namespace FSH.Starter.WebApi.Store.Application.StockLevels.Specs;

/// <summary>
/// Specification to find stock levels by item ID, warehouse ID, and optional warehouse location ID.
/// </summary>
public class StockLevelsByItemWarehouseAndLocationSpec : Specification<StockLevel>
{
    public StockLevelsByItemWarehouseAndLocationSpec(
        DefaultIdType itemId, 
        DefaultIdType warehouseId, 
        DefaultIdType? warehouseLocationId)
    {
        Query
            .Where(sl => 
                sl.ItemId == itemId && 
                sl.WarehouseId == warehouseId &&
                sl.WarehouseLocationId == warehouseLocationId);
    }
}

