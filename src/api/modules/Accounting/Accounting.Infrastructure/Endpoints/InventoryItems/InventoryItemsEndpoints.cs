using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.InventoryItems.Create.v1;
using Accounting.Application.InventoryItems.Get;
using Accounting.Application.InventoryItems.Responses;
using Accounting.Application.InventoryItems.Update.v1;
using Accounting.Application.InventoryItems.Search.v1;
using Accounting.Application.InventoryItems.AddStock.v1;
using Accounting.Application.InventoryItems.ReduceStock.v1;
using Accounting.Application.InventoryItems.Deactivate.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems;

public class InventoryItemsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/inventory-items").WithTags("inventory-items");

        // CRUD operations
        group.MapPost("/", async (CreateInventoryItemCommand command, ISender mediator) =>
        {
            var itemId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/inventory-items/{itemId}", new { Id = itemId });
        })
        .WithName("CreateInventoryItem")
        .WithSummary("Create inventory item")
        .WithDescription("Creates a new inventory item")
        .Produces<object>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetInventoryItemRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInventoryItem")
        .WithSummary("Get inventory item by ID")
        .WithDescription("Gets the details of an inventory item by its ID")
        .Produces<InventoryItemResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInventoryItemCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var itemId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = itemId });
        })
        .WithName("UpdateInventoryItem")
        .WithSummary("update inventory item")
        .WithDescription("updates an inventory item details")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchInventoryItemsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchInventoryItems")
        .WithSummary("Search inventory items")
        .WithDescription("Searches inventory items with filters and pagination")
        .Produces<PagedList<InventoryItemResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Workflow operations
        group.MapPost("/{id:guid}/add-stock", async (DefaultIdType id, AddStockCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var itemId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = itemId });
        })
        .WithName("AddInventoryItemStock")
        .WithSummary("add stock")
        .WithDescription("increases inventory quantity")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reduce-stock", async (DefaultIdType id, ReduceStockCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var itemId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = itemId });
        })
        .WithName("ReduceInventoryItemStock")
        .WithSummary("reduce stock")
        .WithDescription("decreases inventory quantity")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, ISender mediator) =>
        {
            var itemId = await mediator.Send(new DeactivateInventoryItemCommand(id)).ConfigureAwait(false);
            return Results.Ok(new { Id = itemId, Message = "Inventory item deactivated successfully" });
        })
        .WithName("DeactivateInventoryItem")
        .WithSummary("Deactivate inventory item")
        .WithDescription("Deactivates an inventory item")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
