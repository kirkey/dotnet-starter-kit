using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Cancel.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class CancelInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapCancelInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(CancelInventoryTransferEndpoint))
        .WithSummary("Cancel inventory transfer")
        .WithDescription("Cancels a pending or approved inventory transfer with optional reason")
        .Produces<CancelInventoryTransferResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Cancel")
        .MapToApiVersion(1);
    }
}

