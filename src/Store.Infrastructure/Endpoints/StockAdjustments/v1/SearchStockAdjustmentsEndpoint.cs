namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class SearchStockAdjustmentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchStockAdjustmentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // TODO: Implement SearchStockAdjustmentsQuery and handler in the application layer.
        // Until then return 501 Not Implemented to keep project compiling.
        return endpoints.MapGet("/", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
            .WithName("SearchStockAdjustments")
            .WithSummary("Search stock adjustments (not implemented)")
            .WithDescription("Search endpoint pending implementation in the application layer")
            .MapToApiVersion(1);
    }
}
