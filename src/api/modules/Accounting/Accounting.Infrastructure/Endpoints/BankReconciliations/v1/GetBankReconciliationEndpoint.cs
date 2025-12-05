using Accounting.Application.BankReconciliations.Get.v1;
using Accounting.Application.BankReconciliations.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

/// <summary>
/// Endpoint to retrieve a single bank reconciliation by ID.
/// </summary>
public static class GetBankReconciliationEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for retrieving a specific bank reconciliation.
    /// </summary>
    internal static RouteHandlerBuilder MapGetBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBankReconciliationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBankReconciliationEndpoint))
            .WithSummary("Get bank reconciliation details")
            .WithDescription("Retrieve a specific bank reconciliation with all its details including status, balances, " +
                "adjustments, and audit information.")
            .Produces<BankReconciliationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
