using Accounting.Application.BankReconciliations.Create.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to create a new bank reconciliation.
/// Initializes a reconciliation with opening balances from bank statement and general ledger.
/// </summary>
public static class CreateBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the POST endpoint for creating bank reconciliations.
    /// </summary>
    internal static RouteHandlerBuilder MapCreateBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBankReconciliationCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/bank-reconciliations/{response}", response);
            })
            .WithName(nameof(CreateBankReconciliationEndpoint))
            .WithSummary("Create a new bank reconciliation")
            .WithDescription("Create a new bank reconciliation with opening balances from bank statement and general ledger. " +
                "Reconciliation starts in Pending status and moves through InProgress, Completed, and Approved statuses.")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
