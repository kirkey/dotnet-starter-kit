using FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

public static class GetGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapGetGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetGroceryItemQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetGroceryItemEndpoint))
        .WithSummary("Get grocery item by ID")
        .WithDescription("Retrieves a grocery item by its unique identifier")
        .Produces<GroceryItemResponse>()
        .RequirePermission("Permissions.GroceryItems.View")
        .MapToApiVersion(1);
    }
}
