using Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ARAccountReconcileEndpoint
{
    internal static RouteHandlerBuilder MapARAccountReconcileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileARAccountCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Reconciliation completed successfully" });
            })
            .WithName(nameof(ARAccountReconcileEndpoint))
            .WithSummary("Reconcile with subsidiary ledger")
            .WithDescription("Reconciles AR control account with subsidiary customer ledgers")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

