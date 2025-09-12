namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class UpdateInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/inventory-transfers/{id:guid}", async (Guid id, FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1.UpdateInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("UpdateInventoryTransfer")
        .WithSummary("Update inventory transfer")
        .WithDescription("Updates an existing inventory transfer with the provided details")
        .MapToApiVersion(1);
    }
}
namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class CreateInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/inventory-transfers", async (FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1.CreateInventoryTransferCommand command, ISender sender) =>
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

