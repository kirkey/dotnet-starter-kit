using FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Search.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

/// <summary>
/// Endpoint for searching grocery items with pagination and filtering capabilities.
/// </summary>
public static class SearchGroceryItemsEndpoint
{
    /// <summary>
    /// Maps the search grocery items endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search grocery items endpoint</returns>
    internal static RouteHandlerBuilder MapSearchGroceryItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchGroceryItemsCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchGroceryItemsEndpoint))
            .WithSummary("Search grocery items")
            .WithDescription("Search and filter grocery items with pagination support")
            .Produces<PagedList<GroceryItemResponse>>()
            .RequirePermission("Permissions.GroceryItems.View")
            .MapToApiVersion(1);
    }
}
