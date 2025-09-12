namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class CreateStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/stock-adjustments", async (FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1.CreateStockAdjustmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/stock-adjustments/{result.Id}", result);
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a new stock adjustment for inventory corrections")
        .MapToApiVersion(1);
    }
}
