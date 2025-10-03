using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;

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
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
