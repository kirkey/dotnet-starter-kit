using FSH.Starter.WebApi.Store.Application.PriceLists.Get.v1;
using FSH.Starter.WebApi.Store.Application.PriceLists.Search.v1;

namespace Store.Infrastructure.Endpoints.PriceLists.v1;

/// <summary>
/// Endpoint for searching price lists with pagination and filtering capabilities.
/// </summary>
public static class SearchPriceListsEndpoint
{
    /// <summary>
    /// Maps the search price lists endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search price lists endpoint</returns>
    internal static RouteHandlerBuilder MapSearchPriceListsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender mediator, [FromBody] SearchPriceListsCommand command) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchPriceListsEndpoint))
        .WithSummary("Search price lists")
        .WithDescription("Search and filter price lists with pagination support")
        .Produces<PagedList<GetPriceListResponse>>()
        .RequirePermission("Permissions.PriceLists.View")
        .MapToApiVersion(1);
    }
}
