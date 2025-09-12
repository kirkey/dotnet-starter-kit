

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

public class GetWarehouseLocationListSpecification : Specification<WarehouseLocation>
{
    public GetWarehouseLocationListSpecification(SearchWarehouseLocationsCommand request)
    {
        Query.Include(wl => wl.Warehouse);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            Query.Where(wl => wl.Name!.Contains(request.SearchTerm) ||
                            wl.Code.Contains(request.SearchTerm) ||
                            wl.Aisle.Contains(request.SearchTerm) ||
                            wl.Section.Contains(request.SearchTerm) ||
                            wl.Shelf.Contains(request.SearchTerm));
        }

        if (request.WarehouseId.HasValue)
        {
            Query.Where(wl => wl.WarehouseId == request.WarehouseId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.LocationType))
        {
            Query.Where(wl => wl.LocationType.Contains(request.LocationType));
        }

        if (!string.IsNullOrWhiteSpace(request.Aisle))
        {
            Query.Where(wl => wl.Aisle.Contains(request.Aisle));
        }

        if (request.IsActive.HasValue)
        {
            Query.Where(wl => wl.IsActive == request.IsActive.Value);
        }

        if (request.RequiresTemperatureControl.HasValue)
        {
            Query.Where(wl => wl.RequiresTemperatureControl == request.RequiresTemperatureControl.Value);
        }

        Query.OrderBy(wl => wl.Warehouse.Name).ThenBy(wl => wl.Aisle).ThenBy(wl => wl.Section).ThenBy(wl => wl.Shelf);
    }
}
