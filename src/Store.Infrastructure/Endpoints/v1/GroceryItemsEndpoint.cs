using FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Delete.v1;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Store.Infrastructure.Endpoints.v1;

public class GroceryItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("grocery-items").WithTags("Grocery Items");

        group.MapPost("/", async (CreateGroceryItemCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/grocery-items/{result.Id}", result);
        })
        .WithName("CreateGroceryItem")
        .WithSummary("Create a new grocery item")
        .WithDescription("Creates a new grocery item with inventory tracking");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetGroceryItemQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetGroceryItem")
        .WithSummary("Get grocery item by ID")
        .WithDescription("Retrieves a grocery item by its unique identifier");

        group.MapPut("/{id:guid}", async (Guid id, UpdateGroceryItemCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateGroceryItem")
        .WithSummary("Update grocery item")
        .WithDescription("Updates an existing grocery item");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteGroceryItemCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteGroceryItem")
        .WithSummary("Delete grocery item")
        .WithDescription("Deletes a grocery item from the system");

        group.MapPost("/search", async (SearchGroceryItemsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("SearchGroceryItems")
        .WithSummary("Search grocery items")
        .WithDescription("Search grocery items with filtering and pagination");
    }
}
