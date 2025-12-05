using Accounting.Application.BankReconciliations.Approve.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to approve a completed bank reconciliation.
/// Marks the reconciliation as final and approved.
/// </summary>
public static class ApproveBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the POST endpoint to approve a bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapApproveBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ApproveBankReconciliationEndpoint))
            .WithSummary("Approve a bank reconciliation")
            .WithDescription("Approve a completed bank reconciliation, marking it as final and verified. " +
                "Only reconciliations with Completed status can be approved. Sets IsReconciled to true.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
