// Auto-generated: Warehouses endpoints (Catalog-style static mapping)

namespace Store.Infrastructure.Endpoints.v1;

public static class WarehousesEndpoints
{
    public static void MapWarehousesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("warehouses").WithTags("Warehouses");

        // This file has been split into Catalog-style single-endpoint files:
        // - CreateWarehouseEndpoint.cs
        // - GetWarehouseEndpoint.cs
        // - SearchWarehousesEndpoint.cs
        // - UpdateWarehouseEndpoint.cs
        // - DeleteWarehouseEndpoint.cs
        // The grouped endpoints are now removed for consistency.
    }
}
