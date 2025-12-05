using Store.Infrastructure.Endpoints.StockAdjustments.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.StockAdjustments;

/// <summary>
/// Endpoint configuration for Stock Adjustments module.
/// Provides REST API endpoints for managing stock adjustments.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class StockAdjustmentsEndpoints() : CarterModule("store")
{
    /// <summary>
    /// Maps all Stock Adjustments endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, Search, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/stock-adjustments").WithTags("stock-adjustments");

        group.MapCreateStockAdjustmentEndpoint();
        group.MapUpdateStockAdjustmentEndpoint();
        group.MapDeleteStockAdjustmentEndpoint();
        group.MapGetStockAdjustmentEndpoint();
        group.MapSearchStockAdjustmentsEndpoint();
        group.MapApproveStockAdjustmentEndpoint();
    }
}
