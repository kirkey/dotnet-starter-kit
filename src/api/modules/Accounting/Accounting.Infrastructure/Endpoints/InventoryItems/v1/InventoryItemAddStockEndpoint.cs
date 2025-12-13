using Accounting.Application.InventoryItems.AddStock.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.InventoryItems.v1;

public static class InventoryItemAddStockEndpoint
{
    internal static RouteHandlerBuilder MapInventoryItemAddStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/add-stock", async (DefaultIdType id, AddStockCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var itemId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = itemId });
            })
            .WithName(nameof(InventoryItemAddStockEndpoint))
            .WithSummary("add stock")
            .WithDescription("increases inventory quantity")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

