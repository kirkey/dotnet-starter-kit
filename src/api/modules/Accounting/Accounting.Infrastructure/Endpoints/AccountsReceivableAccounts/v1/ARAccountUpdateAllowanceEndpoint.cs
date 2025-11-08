using Accounting.Application.AccountsReceivableAccounts.UpdateAllowance.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountUpdateAllowanceEndpoint
{
    internal static RouteHandlerBuilder MapArAccountUpdateAllowanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/allowance", async (DefaultIdType id, UpdateARAllowanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId });
            })
            .WithName(nameof(ArAccountUpdateAllowanceEndpoint))
            .WithSummary("Update allowance for doubtful accounts")
            .WithDescription("Updates the allowance for uncollectible receivables")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

