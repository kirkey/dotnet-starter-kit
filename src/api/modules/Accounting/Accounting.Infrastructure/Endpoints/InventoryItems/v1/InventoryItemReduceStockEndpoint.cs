using Accounting.Application.InventoryItems.ReduceStock.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemReduceStockEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemReduceStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reduce-stock", async (DefaultIdType id, ReduceStockCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId });
            })
            .WithName(nameof(InventoryItemReduceStockEndpoint))
            .WithSummary("reduce stock")
            .WithDescription("decreases inventory quantity")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

