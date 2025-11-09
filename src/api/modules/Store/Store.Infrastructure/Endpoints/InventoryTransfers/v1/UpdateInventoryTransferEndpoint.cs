using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

/// <summary>
/// Endpoint for updating an inventory transfer.
/// </summary>
public static class UpdateInventoryTransferEndpoint
{
    /// <summary>
    /// Maps the update inventory transfer endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInventoryTransferCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateInventoryTransferEndpoint))
        .WithSummary("Update inventory transfer")
        .WithDescription("Updates an existing inventory transfer with the provided details")
        .Produces<UpdateInventoryTransferResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Update")
        .MapToApiVersion(1);
    }
}
