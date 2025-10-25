using FSH.Starter.WebApi.Store.Application.InventoryTransactions.UpdateNotes.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

/// <summary>
/// Endpoint for updating notes on an inventory transaction.
/// </summary>
public static class UpdateInventoryTransactionNotesEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInventoryTransactionNotesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/{id:guid}/notes", async (DefaultIdType id, UpdateInventoryTransactionNotesCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID mismatch between route and body.");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInventoryTransactionNotesEndpoint))
            .WithSummary("Update inventory transaction notes")
            .WithDescription("Updates the notes field on an existing inventory transaction for additional documentation.")
            .Produces<UpdateInventoryTransactionNotesResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}

