using Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

public static class ArAccountReconcileEndpoint
{
    internal static RouteHandlerBuilder MapArAccountReconcileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reconcile", async (DefaultIdType id, ReconcileArAccountCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Reconciliation completed successfully" });
            })
            .WithName(nameof(ArAccountReconcileEndpoint))
            .WithSummary("Reconcile with subsidiary ledger")
            .WithDescription("Reconciles AR control account with subsidiary customer ledgers")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

