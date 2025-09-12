namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class GetStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapGetStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/stock-adjustments/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1.GetStockAdjustmentQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetStockAdjustment")
        .WithSummary("Get stock adjustment by ID")
        .WithDescription("Retrieves a stock adjustment by its unique identifier")
        .MapToApiVersion(1);
    }
}

