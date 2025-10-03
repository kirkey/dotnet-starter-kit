using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

public static class GetInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetInventoryTransactionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryTransactionEndpoint))
            .WithSummary("Get an inventory transaction by ID")
            .WithDescription("Retrieves a specific inventory transaction by its unique identifier.")
            .Produces<InventoryTransactionResponse>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
