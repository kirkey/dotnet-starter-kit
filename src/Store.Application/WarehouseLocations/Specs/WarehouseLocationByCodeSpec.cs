namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

public class WarehouseLocationByCodeSpec : Specification<WarehouseLocation>
{
    public WarehouseLocationByCodeSpec(string code)
    {
        Query.Where(wl => wl.Code == code);
    }
}

