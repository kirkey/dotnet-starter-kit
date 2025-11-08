using Accounting.Application.InventoryItems.Create.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemCreateEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInventoryItemCommand command, ISender mediator) =>
            {
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/inventory-items/{itemId}", new { Id = itemId });
            })
            .WithName(nameof(InventoryItemCreateEndpoint))
            .WithSummary("Create inventory item")
            .WithDescription("Creates a new inventory item")
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
