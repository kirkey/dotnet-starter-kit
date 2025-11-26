using Accounting.Application.BankReconciliations.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to update reconciliation items for an in-progress bank reconciliation.
/// </summary>
public static class UpdateBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the PUT endpoint for updating bank reconciliation items.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBankReconciliationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch between URL parameter and request body.");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(UpdateBankReconciliationEndpoint))
            .WithSummary("Update reconciliation items")
            .WithDescription("Update outstanding checks, deposits in transit, and error adjustments for an in-progress reconciliation. " +
                "Calculates adjusted book balance which must eventually match the statement balance.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
