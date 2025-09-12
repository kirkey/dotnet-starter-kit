namespace Store.Infrastructure.Endpoints.v1;

public static class GetGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapGetGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/grocery-items/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1.GetGroceryItemQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetGroceryItem")
        .WithSummary("Get grocery item by ID")
        .WithDescription("Retrieves a grocery item by its unique identifier")
        .MapToApiVersion(1);
    }
}

