using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

/// <summary>
/// Endpoint for updating an inventory transfer item.
/// </summary>
public static class UpdateInventoryTransferItemEndpoint
{
    /// <summary>
    /// Maps the update inventory transfer item endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateInventoryTransferItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, UpdateInventoryTransferItemCommand request, ISender sender) =>
        {
            var command = request with { InventoryTransferId = id, ItemId = itemId };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateInventoryTransferItemEndpoint))
        .WithSummary("Update inventory transfer item")
        .WithDescription("Updates quantity and unit price of an item within an existing inventory transfer")
        .Produces<UpdateInventoryTransferItemResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Update")
        .MapToApiVersion(1);
    }
}

