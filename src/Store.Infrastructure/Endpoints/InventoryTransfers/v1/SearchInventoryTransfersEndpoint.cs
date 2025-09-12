using MediatR;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class SearchInventoryTransfersEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryTransfersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // TODO: Implement SearchInventoryTransfersQuery and handler in the application layer.
        // Until then return 501 Not Implemented to keep project compiling.
        return endpoints.MapGet("/", () => Results.StatusCode(StatusCodes.Status501NotImplemented))
            .WithName("SearchInventoryTransfers")
            .WithSummary("Get list of inventory transfers (not implemented)")
            .WithDescription("Search endpoint pending implementation in the application layer")
            .MapToApiVersion(1);
    }
}
