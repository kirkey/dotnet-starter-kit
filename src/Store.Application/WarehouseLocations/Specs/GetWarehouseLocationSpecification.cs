

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

public class GetWarehouseLocationSpecification : Specification<WarehouseLocation>
{
    public GetWarehouseLocationSpecification(DefaultIdType id)
    {
        Query.Where(wl => wl.Id == id);
        Query.Include(wl => wl.Warehouse);
    }
}
