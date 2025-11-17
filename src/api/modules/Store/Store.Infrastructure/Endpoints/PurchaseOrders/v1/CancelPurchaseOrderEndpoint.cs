using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for cancelling a purchase order.
/// Can cancel orders in Draft, Submitted, or Approved status.
/// </summary>
public static class CancelPurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the cancel purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for cancel purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapCancelPurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/cancel", async (DefaultIdType id, CancelPurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new CancelPurchaseOrderCommand(id, request.CancellationReason);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CancelPurchaseOrderEndpoint))
        .WithSummary("Cancel a purchase order")
        .WithDescription("Cancels a purchase order that hasn't been received yet")
        .Produces<CancelPurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.Store))
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request model for cancel purchase order endpoint.
/// </summary>
public sealed record CancelPurchaseOrderRequest(string? CancellationReason = null);
