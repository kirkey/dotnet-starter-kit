using Store.Infrastructure.Endpoints.StockLevels.v1;

namespace Store.Infrastructure.Endpoints.StockLevels;

/// <summary>
/// Carter module for StockLevels endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class StockLevelsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all StockLevels endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/stock-levels").WithTags("stock-levels");

        // Call the individual endpoint handler methods from v1
        group.MapCreateStockLevelEndpoint();
        group.MapUpdateStockLevelEndpoint();
        group.MapDeleteStockLevelEndpoint();
        group.MapGetStockLevelEndpoint();
        group.MapSearchStockLevelsEndpoint();
        group.MapReserveStockEndpoint();
        group.MapAllocateStockEndpoint();
        group.MapReleaseStockEndpoint();
    }
}
