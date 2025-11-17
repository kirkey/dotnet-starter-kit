using Accounting.Application.BankReconciliations.Complete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to mark a bank reconciliation as complete.
/// Validates that adjusted balances match before transitioning to completed status.
/// </summary>
public static class CompleteBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the POST endpoint to complete a bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapCompleteBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, CompleteBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CompleteBankReconciliationEndpoint))
            .WithSummary("Complete a bank reconciliation")
            .WithDescription("Mark a bank reconciliation as completed. Verifies that the adjusted book balance equals " +
                "the statement balance (within tolerance). Records who completed the reconciliation for audit purposes.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
