using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for updating the quantity of a purchase order line item.
/// </summary>
internal static class UpdatePurchaseOrderItemQuantityEndpoint
{
    /// <summary>
    /// Maps the update purchase order item quantity endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdatePurchaseOrderItemQuantityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}/items/{itemId:guid}/quantity", async (DefaultIdType id, DefaultIdType itemId, UpdatePurchaseOrderItemQuantityCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(UpdatePurchaseOrderItemQuantityEndpoint))
        .WithSummary("Update item quantity")
        .WithDescription("Updates the ordered quantity for a specific purchase order line item. Quantity cannot be less than already received quantity.")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}

