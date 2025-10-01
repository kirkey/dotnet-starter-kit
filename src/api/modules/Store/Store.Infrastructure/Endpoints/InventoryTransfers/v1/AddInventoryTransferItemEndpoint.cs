using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class AddInventoryTransferItemEndpoint
{
    internal static RouteHandlerBuilder MapAddInventoryTransferItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/items", async (DefaultIdType id, AddInventoryTransferItemCommand command, ISender sender) =>
        {
            if (id != command.InventoryTransferId) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(AddInventoryTransferItemEndpoint))
        .WithSummary("Add item to inventory transfer")
        .WithDescription("Adds a grocery item line to an existing inventory transfer")
        .Produces<AddInventoryTransferItemResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Update")
        .MapToApiVersion(1);
    }
}
