using Accounting.Application.GeneralLedgers.Update.v1;

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
            .MapPut("/{id:guid}", async (DefaultIdType id, GeneralLedgerUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var entryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = entryId });
            })
            .WithName(nameof(GeneralLedgerUpdateEndpoint))
            .WithSummary("Update a general ledger entry")
            .WithDescription("Updates general ledger entry details (amounts, memo, USOA class)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
