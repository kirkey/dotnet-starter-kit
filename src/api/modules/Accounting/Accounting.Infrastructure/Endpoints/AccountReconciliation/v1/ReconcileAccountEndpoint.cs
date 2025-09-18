using Accounting.Application.AccountReconciliations.Commands.ReconcileAccount.v1;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliation.v1;

public static class ReconcileAccountEndpoint
{
    internal static RouteHandlerBuilder MapReconcileAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/reconcile", async (ReconcileAccountCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(ReconcileAccountEndpoint))
            .WithSummary("Reconcile an account")
            .WithDescription("Run account reconciliation for a chart of account and its reconciliation lines")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

