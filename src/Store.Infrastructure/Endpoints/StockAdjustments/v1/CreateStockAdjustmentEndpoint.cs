using FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class CreateStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateStockAdjustmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/stock-adjustments/{result.Id}", result);
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a stock adjustment for inventory")
        .MapToApiVersion(1);
    }
}
