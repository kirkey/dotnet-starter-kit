using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class GetInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetInventoryTransferQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfer")
        .WithSummary("Get inventory transfer by ID")
        .WithDescription("Retrieves an inventory transfer by its unique identifier")
        .MapToApiVersion(1);
    }
}
