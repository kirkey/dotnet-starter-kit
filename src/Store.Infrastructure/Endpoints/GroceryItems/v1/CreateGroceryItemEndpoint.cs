using FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

public static class CreateGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateGroceryItemCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/grocery-items/{result.Id}", result);
        })
        .WithName("CreateGroceryItem")
        .WithSummary("Create a new grocery item")
        .WithDescription("Creates a new grocery item with inventory tracking")
        .Produces<CreateGroceryItemResponse>()
        .MapToApiVersion(1);
    }
}
