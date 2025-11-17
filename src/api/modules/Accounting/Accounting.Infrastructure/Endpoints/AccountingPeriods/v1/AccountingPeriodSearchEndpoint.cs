using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAccountingPeriodsRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodSearchEndpoint))
            .WithSummary("search accounting periods")
            .WithDescription("search accounting periods")
            .Produces<PagedList<AccountingPeriodResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
