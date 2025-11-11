namespace FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

/// <summary>
/// Specification to find stock level by item and warehouse.
/// </summary>
public class StockLevelByItemAndWarehouseSpec : Specification<StockLevel>
{
    public StockLevelByItemAndWarehouseSpec(DefaultIdType itemId, DefaultIdType warehouseId)
    {
        Query.Where(x => x.ItemId == itemId && x.WarehouseId == warehouseId);
    }
}

