namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class SearchGoodsReceiptsEndpoint
{
    internal static RouteHandlerBuilder MapSearchGoodsReceiptsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchGoodsReceiptsRequest request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchGoodsReceiptsEndpoint))
            .WithSummary("Search goods receipts")
            .WithDescription("Searches goods receipts with pagination and filtering options.")
            .Produces<PagedList<GoodsReceiptDto>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
