using Accounting.Application.BankReconciliations.Start.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to transition a bank reconciliation to in-progress status.
/// </summary>
public static class StartBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the POST endpoint to start a bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapStartBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/start", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new StartBankReconciliationCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(StartBankReconciliationEndpoint))
            .WithSummary("Start a bank reconciliation")
            .WithDescription("Transition a bank reconciliation from Pending to InProgress status. " +
                "Once started, the user can begin entering reconciliation items such as outstanding checks " +
                "and deposits in transit.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
