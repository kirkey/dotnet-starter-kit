using Accounting.Application.GeneralLedgers.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger.v1;

/// <summary>
/// Endpoint for retrieving a general ledger entry by ID.
/// </summary>
public static class GeneralLedgerGetEndpoint
{
    /// <summary>
    /// Maps the general ledger retrieval endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGeneralLedgerGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GeneralLedgerGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GeneralLedgerGetEndpoint))
            .WithSummary("Get general ledger entry by ID")
            .WithDescription("Retrieves a general ledger entry by its unique identifier")
            .Produces<GeneralLedgerGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
