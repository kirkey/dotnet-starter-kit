using Accounting.Application.AccountReconciliations.Delete.v1;

using Accounting.Application.AccountReconciliations.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for deleting an account reconciliation.
/// </summary>
public static class DeleteAccountReconciliationEndpoint
{
    internal static RouteHandlerBuilder MapDeleteAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccountReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeleteAccountReconciliationEndpoint))
            .WithSummary("Delete Account Reconciliation")
            .WithDescription("Delete an account reconciliation (only if not approved)")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

