using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class CreateInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateInventoryTransferCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/inventory-transfers/{result.Id}", result);
        })
        .WithName("CreateInventoryTransfer")
        .WithSummary("Create a new inventory transfer")
        .WithDescription("Creates a new transfer between warehouses")
        .MapToApiVersion(1);
    }
}
