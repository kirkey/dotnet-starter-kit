using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for removing an item from a purchase order.
/// </summary>
internal static class RemovePurchaseOrderItemEndpoint
{
    /// <summary>
    /// Maps the remove purchase order item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapRemovePurchaseOrderItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}/items/{groceryItemId:guid}", async (DefaultIdType id, DefaultIdType groceryItemId, ISender sender) =>
        {
            var command = new RemovePurchaseOrderItemCommand(id, groceryItemId);
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(RemovePurchaseOrderItemEndpoint))
        .WithSummary("Remove an item from a purchase order")
        .WithDescription("Removes a grocery item line from a purchase order. Only allowed for modifiable orders.")
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

