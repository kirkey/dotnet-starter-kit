using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

public static class DeleteInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapDeleteInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeleteInventoryTransactionCommand { Id = id }).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteInventoryTransactionEndpoint))
            .WithSummary("Delete an inventory transaction")
            .WithDescription("Deletes an existing inventory transaction from the system.")
            .Produces<DeleteInventoryTransactionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);
    }
}
