using FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class CreateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCreateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCategoryCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateCategoryEndpoint))
            .WithSummary("Create a new category")
            .WithDescription("Creates a new category")
            .Produces<CreateCategoryResponse>()
            .RequirePermission("Permissions.Categories.Create")
            .MapToApiVersion(1);
    }
}
