using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Delete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

/// <summary>
/// Endpoint for deleting an inventory transfer.
/// </summary>
public static class DeleteInventoryTransferEndpoint
{
    /// <summary>
    /// Maps the delete inventory transfer endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for delete inventory transfer endpoint</returns>
    internal static RouteHandlerBuilder MapDeleteInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteInventoryTransferCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteInventoryTransferEndpoint))
        .WithSummary("Delete an inventory transfer")
        .WithDescription("Deletes an inventory transfer by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}
