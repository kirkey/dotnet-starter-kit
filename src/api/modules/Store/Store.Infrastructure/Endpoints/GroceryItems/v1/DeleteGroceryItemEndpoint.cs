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
        .WithName(nameof(DeleteGroceryItemEndpoint))
        .WithSummary("Delete grocery item")
        .WithDescription("Deletes a grocery item by its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.Store.Delete")
        .MapToApiVersion(1);
    }
}
