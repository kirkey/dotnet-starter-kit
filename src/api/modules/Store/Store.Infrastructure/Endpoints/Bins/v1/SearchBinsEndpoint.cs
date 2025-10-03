using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

namespace Store.Infrastructure.Endpoints.Bins.v1;

public static class SearchBinsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBinsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBinsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBinsEndpoint))
            .WithSummary("Search bins")
            .WithDescription("Searches for storage bins with pagination and filtering")
            .Produces<PagedList<BinResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
