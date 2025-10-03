using Store.Infrastructure.Endpoints.StockLevels.v1;

namespace Store.Infrastructure.Endpoints.StockLevels;

public static class StockLevelsEndpoints
{
    internal static IEndpointRouteBuilder MapStockLevelsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var stockLevelsGroup = endpoints.MapGroup("/stocklevels")
            .WithTags("StockLevels")
            .WithDescription("Endpoints for managing real-time stock levels with reserve/allocate operations");

        // Standard CRUD operations
        stockLevelsGroup.MapCreateStockLevelEndpoint();
        stockLevelsGroup.MapUpdateStockLevelEndpoint();
        stockLevelsGroup.MapDeleteStockLevelEndpoint();
        stockLevelsGroup.MapGetStockLevelEndpoint();
        stockLevelsGroup.MapSearchStockLevelsEndpoint();

        // Special stock operations
        stockLevelsGroup.MapReserveStockEndpoint();
        stockLevelsGroup.MapAllocateStockEndpoint();
        stockLevelsGroup.MapReleaseStockEndpoint();

        return endpoints;
    }
}
