namespace Store.Infrastructure.Endpoints.v1;

public static class DeleteGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapDeleteGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/grocery-items/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new FSH.Starter.WebApi.Store.Application.GroceryItems.Delete.v1.DeleteGroceryItemCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteGroceryItem")
        .WithSummary("Delete grocery item")
        .WithDescription("Deletes a grocery item by its unique identifier")
        .MapToApiVersion(1);
    }
}
namespace Store.Infrastructure.Endpoints.v1;

public static class CreateGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/grocery-items", async (FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1.CreateGroceryItemCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/grocery-items/{result.Id}", result);
        })
        .WithName("CreateGroceryItem")
        .WithSummary("Create a new grocery item")
        .WithDescription("Creates a new grocery item with inventory tracking")
        .MapToApiVersion(1);
    }
}

