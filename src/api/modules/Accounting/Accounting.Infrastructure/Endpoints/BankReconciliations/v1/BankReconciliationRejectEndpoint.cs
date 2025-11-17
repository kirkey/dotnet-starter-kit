using Accounting.Application.BankReconciliations.Reject.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to reject a completed bank reconciliation for rework.
/// Returns reconciliation to Pending status for corrections.
/// </summary>
public static class RejectBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the POST endpoint to reject a bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapRejectBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, RejectBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(RejectBankReconciliationEndpoint))
            .WithSummary("Reject a bank reconciliation")
            .WithDescription("Reject a completed bank reconciliation and return it to Pending status for rework. " +
                "The rejection reason is recorded in the reconciliation notes for reference.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
