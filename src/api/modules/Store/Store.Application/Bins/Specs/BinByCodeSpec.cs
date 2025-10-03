namespace FSH.Starter.WebApi.Store.Application.Bins.Specs;

public class BinByCodeSpec : Specification<Bin>
{
    public BinByCodeSpec(string code, DefaultIdType warehouseLocationId)
    {
        Query
            .Where(b => b.Code == code && b.WarehouseLocationId == warehouseLocationId);
    }
}
