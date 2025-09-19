using Accounting.Application.AccountingPeriods.Dtos;
using Accounting.Application.AccountingPeriods.Search.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/", async ([AsParameters] SearchAccountingPeriodsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
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
