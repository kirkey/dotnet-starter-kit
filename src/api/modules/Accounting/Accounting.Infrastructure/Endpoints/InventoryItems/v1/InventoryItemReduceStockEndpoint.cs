using Accounting.Application.InventoryItems.ReduceStock.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemReduceStockEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemReduceStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reduce-stock", async (DefaultIdType id, ReduceStockCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId, Message = "Stock reduced successfully" });
            })
            .WithName(nameof(InventoryItemReduceStockEndpoint))
            .WithSummary("Reduce stock")
            .WithDescription("Decreases inventory quantity")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

