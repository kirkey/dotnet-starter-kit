using Accounting.Application.AccountingPeriods.Close.v1;
using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;

public static class AccountingPeriodCloseEndpoint
{
    internal static RouteHandlerBuilder MapAccountingPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapPost("/{id:guid}/close", async (DefaultIdType id, AccountingPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var response = await mediator.Send(command).ConfigureAwait(false);
                return TypedResults.Ok(response);
            })
            .WithName(nameof(AccountingPeriodCloseEndpoint))
            .WithSummary("Close accounting period")
            .WithDescription("Closes an accounting period")
            .Produces<AccountingPeriodTransitionResponse>();
}

