using Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountUpdateBalanceEndpoint
{
    internal static RouteHandlerBuilder MapArAccountUpdateBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/balance", async (DefaultIdType id, UpdateARBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName(nameof(ArAccountUpdateBalanceEndpoint))
            .WithSummary("Update AR aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

