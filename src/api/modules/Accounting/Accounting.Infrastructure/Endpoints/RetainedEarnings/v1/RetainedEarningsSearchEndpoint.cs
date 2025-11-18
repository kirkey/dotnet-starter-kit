using Accounting.Application.RetainedEarnings.Responses;
using Accounting.Application.RetainedEarnings.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsSearchEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchRetainedEarningsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RetainedEarningsSearchEndpoint))
            .WithSummary("Search retained earnings")
            .Produces<List<RetainedEarningsResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


