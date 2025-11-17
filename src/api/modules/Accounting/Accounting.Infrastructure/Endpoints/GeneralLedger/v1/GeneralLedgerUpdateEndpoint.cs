using Accounting.Application.GeneralLedgers.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger.v1;

/// <summary>
/// Endpoint for updating a general ledger entry.
/// </summary>
public static class GeneralLedgerUpdateEndpoint
{
    /// <summary>
    /// Maps the general ledger update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGeneralLedgerUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, GeneralLedgerUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var entryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = entryId });
            })
            .WithName(nameof(GeneralLedgerUpdateEndpoint))
            .WithSummary("Update a general ledger entry")
            .WithDescription("Updates general ledger entry details (amounts, memo, USOA class)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
