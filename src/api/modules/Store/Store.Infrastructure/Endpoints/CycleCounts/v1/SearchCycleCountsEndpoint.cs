using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;
using FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class SearchCycleCountsEndpoint
{
    internal static RouteHandlerBuilder MapSearchCycleCountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (SearchCycleCountsRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(SearchCycleCountsEndpoint))
        .WithSummary("Search cycle counts")
        .WithDescription("Searches cycle counts with pagination and filters")
        .Produces<PagedList<CycleCountResponse>>()
        .MapToApiVersion(1);
    }
}
