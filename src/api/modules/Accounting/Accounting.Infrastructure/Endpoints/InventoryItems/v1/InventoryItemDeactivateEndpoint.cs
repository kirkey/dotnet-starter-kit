using Accounting.Application.InventoryItems.Deactivate.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemDeactivateEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemDeactivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var itemId = await mediator.Send(new DeactivateInventoryItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId, Message = "Inventory item deactivated successfully" });
            })
            .WithName(nameof(InventoryItemDeactivateEndpoint))
            .WithSummary("Deactivate inventory item")
            .WithDescription("Deactivates an inventory item")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

