using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Search.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAccountingPeriodsQuery request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodSearchEndpoint))
            .WithSummary("search accounting periods")
            .WithDescription("search accounting periods")
            .Produces<PagedList<AccountingPeriodResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
