using FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

/// <summary>
/// Endpoint for searching cycle count items.
/// </summary>
public static class SearchCycleCountItemsEndpoint
{
    internal static RouteHandlerBuilder MapSearchCycleCountItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/items/search", async (SearchCycleCountItemsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(SearchCycleCountItemsEndpoint))
        .WithSummary("Search cycle count items")
        .WithDescription("Search cycle count items with detailed item information for mobile counting")
        .Produces<PagedList<CycleCountItemDetailResponse>>()
        .MapToApiVersion(1);
    }
}

