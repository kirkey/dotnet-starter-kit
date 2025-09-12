// Auto-generated: Warehouse Locations endpoints (Catalog-style static mapping)

namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class WarehouseLocationsEndpoints
{
    public static void MapWarehouseLocationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("warehouse-locations").WithTags("Warehouse Locations");

        // This file has been split into Catalog-style single-endpoint files:
        // - CreateWarehouseLocationEndpoint.cs
        // - GetWarehouseLocationEndpoint.cs
        // - SearchWarehouseLocationsEndpoint.cs
        // - UpdateWarehouseLocationEndpoint.cs
        // The grouped endpoints are now removed for consistency
    }
}
