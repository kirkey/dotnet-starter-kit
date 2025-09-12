namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class GetInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/inventory-transfers/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1.GetInventoryTransferQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfer")
        .WithSummary("Get inventory transfer by ID")
        .WithDescription("Retrieves an inventory transfer by its unique identifier")
        .MapToApiVersion(1);
    }
}

