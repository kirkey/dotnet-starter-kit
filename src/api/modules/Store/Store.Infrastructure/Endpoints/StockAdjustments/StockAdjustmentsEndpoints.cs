namespace Store.Infrastructure.Endpoints.StockAdjustments;

/// <summary>
/// Endpoint configuration for Stock Adjustments module.
/// </summary>
public static class StockAdjustmentsEndpoints
{
    /// <summary>
    /// Maps all Stock Adjustments endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapStockAdjustmentsEndpoints(this IEndpointRouteBuilder app)
    {
        var stockAdjustmentsGroup = app.MapGroup("/stock-adjustments")
            .WithTags("Stock-Adjustments")
            .WithDescription("Endpoints for managing inventory stock adjustments");

        // Version 1 endpoints will be added here when implemented
        // stockAdjustmentsGroup.MapCreateStockAdjustmentEndpoint();
        // stockAdjustmentsGroup.MapUpdateStockAdjustmentEndpoint();
        // stockAdjustmentsGroup.MapDeleteStockAdjustmentEndpoint();
        // stockAdjustmentsGroup.MapGetStockAdjustmentEndpoint();
        // stockAdjustmentsGroup.MapSearchStockAdjustmentsEndpoint();

        return app;
    }
}
