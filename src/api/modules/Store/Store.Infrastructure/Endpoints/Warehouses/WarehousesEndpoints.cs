using Store.Infrastructure.Endpoints.Warehouses.v1;

namespace Store.Infrastructure.Endpoints.Warehouses;

/// <summary>
/// Endpoint configuration for Warehouses module.
/// </summary>
public static class WarehousesEndpoints
{
    /// <summary>
    /// Maps all Warehouses endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapWarehousesEndpoints(this IEndpointRouteBuilder app)
    {
        var warehousesGroup = app.MapGroup("/warehouses")
            .WithTags("Warehouses")
            .WithDescription("Endpoints for managing warehouses");

        // Version 1 endpoints
        warehousesGroup.MapCreateWarehouseEndpoint();
        warehousesGroup.MapUpdateWarehouseEndpoint();
        warehousesGroup.MapDeleteWarehouseEndpoint();
        warehousesGroup.MapGetWarehouseEndpoint();
        warehousesGroup.MapSearchWarehousesEndpoint();

        return app;
    }
}
