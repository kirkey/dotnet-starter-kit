using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Remove.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class RemoveInventoryTransferItemEndpoint
{
    internal static RouteHandlerBuilder MapRemoveInventoryTransferItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, ISender sender) =>
        {
            await sender.Send(new RemoveInventoryTransferItemCommand(id, itemId)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(RemoveInventoryTransferItemEndpoint))
        .WithSummary("Remove item from inventory transfer")
        .WithDescription("Removes a grocery item line from an existing inventory transfer")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}
