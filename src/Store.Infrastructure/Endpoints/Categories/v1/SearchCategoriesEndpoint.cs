using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class SearchCategoriesEndpoint
{
    internal static RouteHandlerBuilder MapSearchCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCategoriesCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCategoriesEndpoint))
            .WithSummary("Search categories")
            .WithDescription("Searches categories with pagination and filters")
            .Produces<PagedList<CategoryResponse>>()
            .RequirePermission("Permissions.Categories.View")
            .MapToApiVersion(1);
    }
}
