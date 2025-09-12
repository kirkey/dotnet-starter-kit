using Store.Application.StockAdjustments.Get.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class GetStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapGetStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetStockAdjustmentQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetStockAdjustment")
        .WithSummary("Get stock adjustment by ID")
        .WithDescription("Retrieves a stock adjustment by its unique identifier")
        .MapToApiVersion(1);
    }
}
