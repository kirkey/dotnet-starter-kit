using FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

public static class CreateGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateGroceryItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateGroceryItemEndpoint))
            .WithSummary("Create a new grocery item")
            .WithDescription("Creates a new grocery item with inventory tracking")
            .Produces<CreateGroceryItemResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
