using Accounting.Application.InventoryItems.AddStock.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemAddStockEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemAddStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/add-stock", async (DefaultIdType id, AddStockCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId, Message = "Stock added successfully" });
            })
            .WithName(nameof(InventoryItemAddStockEndpoint))
            .WithSummary("Add stock")
            .WithDescription("Increases inventory quantity")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

