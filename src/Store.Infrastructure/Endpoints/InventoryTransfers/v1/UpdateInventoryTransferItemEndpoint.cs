using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class UpdateInventoryTransferItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInventoryTransferItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, UpdateInventoryTransferItemCommand command, ISender sender) =>
        {
            if (id != command.InventoryTransferId) return Results.BadRequest("InventoryTransferId mismatch");
            if (itemId != command.ItemId) return Results.BadRequest("ItemId mismatch");
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

