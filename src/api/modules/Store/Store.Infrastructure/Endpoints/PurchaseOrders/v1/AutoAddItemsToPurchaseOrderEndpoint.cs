using FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for automatically adding items needing reorder to a purchase order.
/// Adds all items at or below reorder point from the purchase order's supplier.
/// </summary>
public static class AutoAddItemsToPurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the auto-add items to purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for auto-add items endpoint</returns>
    internal static RouteHandlerBuilder MapAutoAddItemsToPurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/auto-add-items",
            async (DefaultIdType id, [FromBody] AutoAddItemsToPurchaseOrderCommand command, ISender sender) =>
            {
                // Override purchase order ID from route parameter
                var cmd = command with { PurchaseOrderId = id };
                var result = await sender.Send(cmd).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(AutoAddItemsToPurchaseOrderEndpoint))
            .WithSummary("Auto-add items needing reorder to purchase order")
            .WithDescription("Automatically adds items that are at or below their reorder point to the specified purchase order based on the order's supplier. Only works on Draft status purchase orders.")
            .Produces<AutoAddItemsToPurchaseOrderResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}

