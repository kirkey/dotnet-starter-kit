using FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

/// <summary>
/// Endpoint for updating a stock adjustment.
/// </summary>
public static class UpdateStockAdjustmentEndpoint
{
    /// <summary>
    /// Maps the update stock adjustment endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for update stock adjustment endpoint</returns>
    internal static RouteHandlerBuilder MapUpdateStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateStockAdjustmentCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateStockAdjustmentEndpoint))
        .WithSummary("Update a stock adjustment")
        .WithDescription("Updates an existing stock adjustment")
        .Produces<UpdateStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}
