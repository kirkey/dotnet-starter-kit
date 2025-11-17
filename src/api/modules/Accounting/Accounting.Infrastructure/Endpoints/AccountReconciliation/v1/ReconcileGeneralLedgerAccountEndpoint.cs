using Accounting.Application.AccountReconciliations.Commands.ReconcileAccount.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliation.v1;

/// <summary>
/// Endpoint for reconciling general ledger accounts.
/// </summary>
public static class ReconcileGeneralLedgerAccountEndpoint
{
    internal static RouteHandlerBuilder MapReconcileGeneralLedgerAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/reconcile", async (ReconcileGeneralLedgerAccountCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(ReconcileGeneralLedgerAccountEndpoint))
            .WithSummary("Reconcile a general ledger account")
            .WithDescription("Run account reconciliation for a chart of account and its reconciliation lines")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

