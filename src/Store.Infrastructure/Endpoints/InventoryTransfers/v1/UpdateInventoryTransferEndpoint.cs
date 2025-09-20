using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class UpdateInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
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
