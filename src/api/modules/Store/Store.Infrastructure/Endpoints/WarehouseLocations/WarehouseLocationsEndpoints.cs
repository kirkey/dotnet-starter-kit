namespace Store.Infrastructure.Endpoints.WarehouseLocations;

/// <summary>
/// Endpoint configuration for Warehouse Locations module.
/// </summary>
public static class WarehouseLocationsEndpoints
{
    /// <summary>
    /// Maps all Warehouse Locations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapWarehouseLocationsEndpoints(this IEndpointRouteBuilder app)
    {
        var warehouseLocationsGroup = app.MapGroup("/warehouse-locations")
            .WithTags("Warehouse-Locations")
            .WithDescription("Endpoints for managing warehouse locations");

        // Version 1 endpoints will be added here when implemented
        // warehouseLocationsGroup.MapCreateWarehouseLocationEndpoint();
        // warehouseLocationsGroup.MapUpdateWarehouseLocationEndpoint();
        // warehouseLocationsGroup.MapDeleteWarehouseLocationEndpoint();
        // warehouseLocationsGroup.MapGetWarehouseLocationEndpoint();
        // warehouseLocationsGroup.MapSearchWarehouseLocationsEndpoint();

        return app;
    }
}
