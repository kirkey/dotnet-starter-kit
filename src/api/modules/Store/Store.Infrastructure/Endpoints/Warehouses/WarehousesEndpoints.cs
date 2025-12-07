using Store.Infrastructure.Endpoints.Warehouses.v1;

namespace Store.Infrastructure.Endpoints.Warehouses;

/// <summary>
/// Carter module for Warehouses endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class WarehousesEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Warehouses endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/warehouses").WithTags("warehouses");

        // Call the individual endpoint handler methods from v1
        group.MapCreateWarehouseEndpoint();
        group.MapUpdateWarehouseEndpoint();
        group.MapDeleteWarehouseEndpoint();
        group.MapGetWarehouseEndpoint();
        group.MapSearchWarehousesEndpoint();
        group.MapAssignWarehouseManagerEndpoint();
    }
}
