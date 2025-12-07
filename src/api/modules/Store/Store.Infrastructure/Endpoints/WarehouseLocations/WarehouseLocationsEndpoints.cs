using Store.Infrastructure.Endpoints.WarehouseLocations.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.WarehouseLocations;

/// <summary>
/// Endpoint configuration for Warehouse Locations module.
/// Provides REST API endpoints for managing warehouse locations.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class WarehouseLocationsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Warehouse Locations endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and Search operations.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/warehouse-locations").WithTags("warehouse-locations");

        group.MapCreateWarehouseLocationEndpoint();
        group.MapUpdateWarehouseLocationEndpoint();
        group.MapDeleteWarehouseLocationEndpoint();
        group.MapGetWarehouseLocationEndpoint();
        group.MapSearchWarehouseLocationsEndpoint();
    }
}
