using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for recording received quantity for a purchase order line item.
/// </summary>
internal static class ReceivePurchaseOrderItemQuantityEndpoint
{
    /// <summary>
    /// Maps the receive purchase order item quantity endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapReceivePurchaseOrderItemQuantityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}/items/{itemId:guid}/receive", async (DefaultIdType id, DefaultIdType itemId, ReceivePurchaseOrderItemQuantityCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(ReceivePurchaseOrderItemQuantityEndpoint))
        .WithSummary("Record received quantity for an item")
        .WithDescription("Sets the received quantity for a purchase order line item (can be partial or complete).")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}

