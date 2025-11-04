using Accounting.Application.BankReconciliations.Responses;
using Accounting.Application.BankReconciliations.Search.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to search and filter bank reconciliations with pagination support.
/// </summary>
public static class SearchBankReconciliationsEndpoint
{
    /// <summary>
    /// Maps the POST endpoint to search bank reconciliations.
    /// </summary>
    internal static RouteHandlerBuilder MapSearchBankReconciliationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBankReconciliationsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBankReconciliationsEndpoint))
            .WithSummary("Search bank reconciliations")
            .WithDescription("Search and filter bank reconciliations by bank account, date range, status, and reconciliation state. " +
                "Supports pagination. Results are ordered by reconciliation date (most recent first).")
            .Produces<PagedList<BankReconciliationResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
