using Accounting.Application.AccountsPayableAccounts.Reconcile.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountReconcileEndpoint
{
    internal static RouteHandlerBuilder MapApAccountReconcileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ReconcileAPAccountCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Reconciliation completed successfully" });
            })
            .WithName(nameof(ApAccountReconcileEndpoint))
            .WithSummary("Reconcile with subsidiary ledger")
            .WithDescription("Reconciles AP control account with subsidiary vendor ledgers")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

