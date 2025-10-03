using FSH.Starter.WebApi.Store.Application.Items.Create.v1;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class CreateItemEndpoint
{
    internal static RouteHandlerBuilder MapCreateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateItemEndpoint))
            .WithSummary("Create a new item")
            .WithDescription("Creates a new inventory item")
            .Produces<CreateItemResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
