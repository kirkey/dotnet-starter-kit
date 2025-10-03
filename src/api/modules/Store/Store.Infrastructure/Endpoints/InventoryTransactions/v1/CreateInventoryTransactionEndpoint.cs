using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Create.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

public static class CreateInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInventoryTransactionCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInventoryTransactionEndpoint))
            .WithSummary("Create a new inventory transaction")
            .WithDescription("Creates a new inventory transaction for stock movement tracking and audit trail.")
            .Produces<CreateInventoryTransactionResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
