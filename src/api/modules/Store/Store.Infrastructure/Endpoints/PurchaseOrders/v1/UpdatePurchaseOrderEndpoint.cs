using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for updating a purchase order.
/// </summary>
public static class UpdatePurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the update purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for update purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapUpdatePurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePurchaseOrderCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdatePurchaseOrderEndpoint))
        .WithSummary("Update a purchase order")
        .WithDescription("Updates an existing purchase order")
        .Produces<UpdatePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}
