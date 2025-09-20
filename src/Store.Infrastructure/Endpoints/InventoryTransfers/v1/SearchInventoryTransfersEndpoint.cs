using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class SearchInventoryTransfersEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryTransfersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender sender, [FromBody] SearchInventoryTransfersCommand request) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(SearchInventoryTransfersEndpoint))
        .WithSummary("Get list of inventory transfers")
        .WithDescription("Retrieves a paginated list of inventory transfers with optional filtering")
        .Produces<PagedList<GetInventoryTransferListResponse>>()
        .RequirePermission("Permissions.InventoryTransfers.View")
        .MapToApiVersion(1);
    }
}
