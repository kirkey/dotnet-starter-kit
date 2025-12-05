using Accounting.Application.Accruals.Search;
using Accounting.Application.Accruals.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class SearchAccrualsEndpoint
{
    internal static RouteHandlerBuilder MapSearchAccrualsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (SearchAccrualsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchAccrualsEndpoint))
        .WithSummary("Search accruals")
        .WithDescription("Search accrual entries with filters and pagination")
        .Produces<PagedList<AccrualResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
