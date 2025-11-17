using FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class SearchPickListsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPickListsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPickListsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPickListsEndpoint))
            .WithSummary("Search pick lists")
            .WithDescription("Searches pick lists with pagination and filtering options.")
            .Produces<PagedList<PickListResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
