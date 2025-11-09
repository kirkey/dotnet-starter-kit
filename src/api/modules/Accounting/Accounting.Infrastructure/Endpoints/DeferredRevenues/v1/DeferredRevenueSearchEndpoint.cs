using Accounting.Application.DeferredRevenues.Responses;
using Accounting.Application.DeferredRevenues.Search;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenues.v1;

public static class DeferredRevenueSearchEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDeferredRevenuesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeferredRevenueSearchEndpoint))
            .WithSummary("Search deferred revenues")
            .WithDescription("Searches deferred revenue entries with filtering and pagination")
            .Produces<PagedList<DeferredRevenueResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

