using Accounting.Application.AccountingPeriods.Reopen.v1;
using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodReopenEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodReopenEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id:guid}/reopen", async (DefaultIdType id, AccountingPeriodReopenCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var response = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(response);
            })
            .WithName(nameof(AccountingPeriodReopenEndpoint))
            .WithSummary("Reopen accounting period")
            .WithDescription("Reopens a previously closed accounting period")
            .Produces<AccountingPeriodTransitionResponse>();
}

