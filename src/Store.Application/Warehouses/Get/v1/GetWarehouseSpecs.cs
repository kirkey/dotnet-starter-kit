

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public class GetWarehouseSpecs : Specification<Warehouse, WarehouseResponse>
{
    public GetWarehouseSpecs(DefaultIdType id)
    {
        Query
            .Where(w => w.Id == id);
    }
}
