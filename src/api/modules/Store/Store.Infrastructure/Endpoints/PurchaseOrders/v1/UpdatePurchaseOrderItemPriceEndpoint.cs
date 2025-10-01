using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdatePrice.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for updating the unit price and discount of a purchase order line item.
/// </summary>
internal static class UpdatePurchaseOrderItemPriceEndpoint
{
    /// <summary>
    /// Maps the update purchase order item price endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdatePurchaseOrderItemPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}/items/{itemId:guid}/price", async (DefaultIdType id, DefaultIdType itemId, UpdatePurchaseOrderItemPriceCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(UpdatePurchaseOrderItemPriceEndpoint))
        .WithSummary("Update item price and discount")
        .WithDescription("Updates the unit price and optional discount on a specific purchase order line item.")
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

