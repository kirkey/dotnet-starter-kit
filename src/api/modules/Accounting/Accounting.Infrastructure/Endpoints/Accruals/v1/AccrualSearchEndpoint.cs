using Accounting.Application.Accruals.Responses;
using Accounting.Application.Accruals.Search;
using Shared.Authorization;

// Endpoint for searching accruals
namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccrualSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchAccrualsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualSearchEndpoint))
            .WithSummary("Search accruals")
            .WithDescription("Search accrual entries with filters and pagination")
            .Produces<PagedList<AccrualResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
