using FSH.Starter.WebApi.Store.Application.Items.Get.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class SearchItemsEndpoint
{
    internal static RouteHandlerBuilder MapSearchItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchItemsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchItemsEndpoint))
            .WithSummary("Search items")
            .WithDescription("Searches for inventory items with pagination and filtering")
            .Produces<PagedList<ItemResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
