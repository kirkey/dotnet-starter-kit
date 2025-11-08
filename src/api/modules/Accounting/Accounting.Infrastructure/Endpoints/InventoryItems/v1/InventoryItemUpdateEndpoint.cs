using Accounting.Application.InventoryItems.Update.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemUpdateEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateInventoryItemCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId });
            })
            .WithName(nameof(InventoryItemUpdateEndpoint))
            .WithSummary("update inventory item")
            .WithDescription("updates an inventory item details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

