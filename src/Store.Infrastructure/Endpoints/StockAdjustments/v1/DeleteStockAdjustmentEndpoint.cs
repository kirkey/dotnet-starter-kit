using FSH.Starter.WebApi.Store.Application.StockAdjustments.Delete.v1;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

/// <summary>
/// Endpoint for deleting a stock adjustment.
/// </summary>
public static class DeleteStockAdjustmentEndpoint
{
    /// <summary>
    /// Maps the delete stock adjustment endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for delete stock adjustment endpoint</returns>
    internal static RouteHandlerBuilder MapDeleteStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteStockAdjustmentCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteStockAdjustment")
        .WithSummary("Delete a stock adjustment")
        .WithDescription("Deletes a stock adjustment by ID")
        .Produces(204)
        .MapToApiVersion(1);
    }
}
