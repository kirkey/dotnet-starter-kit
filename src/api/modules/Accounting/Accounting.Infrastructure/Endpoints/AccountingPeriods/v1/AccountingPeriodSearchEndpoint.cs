using Accounting.Application.AccountingPeriods.Dtos;
using Accounting.Application.AccountingPeriods.Search.v1;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAccountingPeriodsRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccountingPeriodSearchEndpoint))
            .WithSummary("Gets a list of accounting periods")
            .WithDescription("Gets a list of accounting periods with pagination and filtering support")
            .Produces<PagedList<AccountingPeriodDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


