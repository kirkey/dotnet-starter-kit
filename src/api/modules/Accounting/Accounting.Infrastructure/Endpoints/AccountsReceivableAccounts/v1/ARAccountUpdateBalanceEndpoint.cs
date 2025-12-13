using Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountUpdateBalanceEndpoint
{
    internal static RouteHandlerBuilder MapArAccountUpdateBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/balance", async (DefaultIdType id, UpdateArBalanceCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName(nameof(ArAccountUpdateBalanceEndpoint))
            .WithSummary("Update AR aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

