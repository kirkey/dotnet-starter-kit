using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Reject.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

/// <summary>
/// Endpoint for rejecting an inventory transaction.
/// </summary>
public static class RejectInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapRejectInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, RejectInventoryTransactionCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID mismatch between route and body.");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectInventoryTransactionEndpoint))
            .WithSummary("Reject an inventory transaction")
            .WithDescription("Rejects a previously approved inventory transaction, optionally recording the reason for rejection.")
            .Produces<RejectInventoryTransactionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}

