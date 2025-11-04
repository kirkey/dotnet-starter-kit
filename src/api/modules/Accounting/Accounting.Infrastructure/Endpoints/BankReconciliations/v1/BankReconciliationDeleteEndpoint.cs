using Accounting.Application.BankReconciliations.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to delete a bank reconciliation.
/// Only reconciliations in Pending or InProgress status can be deleted.
/// </summary>
public static class DeleteBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the DELETE endpoint for removing a bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapDeleteBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeleteBankReconciliationEndpoint))
            .WithSummary("Delete a bank reconciliation")
            .WithDescription("Delete a bank reconciliation. Only reconciliations that are not yet reconciled and approved can be deleted. " +
                "This permanently removes the reconciliation record.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
