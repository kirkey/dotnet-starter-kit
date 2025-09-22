using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for adding an item to an existing purchase order.
/// </summary>
internal static class AddPurchaseOrderItemEndpoint
{
    /// <summary>
    /// Maps the add purchase order item endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapAddPurchaseOrderItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/items", async (DefaultIdType id, AddPurchaseOrderItemCommand request, ISender sender) =>
        {
            // Attach the path purchase order id to the command and forward to MediatR
            var command = request with { PurchaseOrderId = id };
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(AddPurchaseOrderItemEndpoint))
        .WithSummary("Add an item to a purchase order")
        .WithDescription("Adds a grocery item line to an existing purchase order. If the item already exists the aggregate will increase the quantity.")
        .Produces<AddPurchaseOrderItemResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}

