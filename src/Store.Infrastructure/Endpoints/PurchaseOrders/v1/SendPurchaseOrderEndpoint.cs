using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for sending an approved purchase order to the supplier.
/// Moves order from Approved to Sent status.
/// </summary>
public static class SendPurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the send purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for send purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapSendPurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/send", async (DefaultIdType id, SendPurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new SendPurchaseOrderCommand(id, request.DeliveryInstructions);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SendPurchaseOrderEndpoint))
        .WithSummary("Send an approved purchase order to supplier")
        .WithDescription("Sends an approved purchase order to the supplier, changing status from Approved to Sent")
        .Produces<SendPurchaseOrderResponse>()
        .RequirePermission("Permissions.Store.Send")
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request model for send purchase order endpoint.
/// </summary>
public sealed record SendPurchaseOrderRequest(string? DeliveryInstructions = null);
