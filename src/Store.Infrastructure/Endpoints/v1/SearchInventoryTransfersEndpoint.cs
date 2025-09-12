namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class SearchInventoryTransfersEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryTransfersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/inventory-transfers", async ([AsParameters] FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1.GetInventoryTransferListQuery query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfers")
        .WithSummary("Get list of inventory transfers")
        .WithDescription("Retrieves a paginated list of inventory transfers with optional filtering")
        .MapToApiVersion(1);
    }
}

