using FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

/// <summary>
/// Endpoint for updating notes on an inventory transaction.
/// </summary>
public static class UpdateInventoryTransactionNotesEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInventoryTransactionNotesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/{id}/notes", async (DefaultIdType id, UpdateInventoryTransactionNotesCommand request, ISender sender) =>
            {
                request.Id = id;
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInventoryTransactionNotesEndpoint))
            .WithSummary("Update inventory transaction notes")
            .WithDescription("Updates the notes field on an existing inventory transaction for additional documentation.")
            .Produces<UpdateInventoryTransactionNotesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}

