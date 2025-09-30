using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for receiving a purchase order delivery.
/// Marks the entire order as received and updates delivery date.
/// </summary>
public static class ReceivePurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the receive purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for receive purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapReceivePurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/receive", async (DefaultIdType id, ReceivePurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new ReceivePurchaseOrderCommand(id, request.ActualDeliveryDate, request.ReceiptNotes);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(ReceivePurchaseOrderEndpoint))
        .WithSummary("Receive a purchase order delivery")
        .WithDescription("Marks a sent purchase order as received and records the actual delivery date")
        .Produces<ReceivePurchaseOrderResponse>()
        .RequirePermission("Permissions.Store.Receive")
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request model for receive purchase order endpoint.
/// </summary>
public sealed record ReceivePurchaseOrderRequest(DateTime? ActualDeliveryDate = null, string? ReceiptNotes = null);
