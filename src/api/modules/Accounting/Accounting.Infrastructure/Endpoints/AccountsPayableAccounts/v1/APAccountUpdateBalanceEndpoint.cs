using Accounting.Application.AccountsPayableAccounts.UpdateBalance.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountUpdateBalanceEndpoint
{
    internal static RouteHandlerBuilder MapApAccountUpdateBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/balance", async (DefaultIdType id, UpdateAPBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName(nameof(ApAccountUpdateBalanceEndpoint))
            .WithSummary("Update AP aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

