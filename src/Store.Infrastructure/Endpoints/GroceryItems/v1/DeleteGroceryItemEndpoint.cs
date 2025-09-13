using FSH.Starter.WebApi.Store.Application.GroceryItems.Delete.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

public static class DeleteGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapDeleteGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteGroceryItemCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteGroceryItem")
        .WithSummary("Delete grocery item")
        .WithDescription("Deletes a grocery item by its unique identifier")
        .MapToApiVersion(1);
    }
}
