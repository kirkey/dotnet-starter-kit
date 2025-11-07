using Accounting.Application.AccountsPayableAccounts.UpdateBalance.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class APAccountUpdateBalanceEndpoint
{
    internal static RouteHandlerBuilder MapAPAccountUpdateBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/balance", async (DefaultIdType id, UpdateAPBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName(nameof(APAccountUpdateBalanceEndpoint))
            .WithSummary("Update AP aging balance")
            .WithDescription("Updates the aging buckets and calculates total balance")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

