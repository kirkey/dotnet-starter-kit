using FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class CreateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCreateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCategoryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/categories/{result.Id}", result);
        })
        .WithName("CreateCategory")
        .WithSummary("Create a new category")
        .WithDescription("Creates a new category")
        .Produces<CreateCategoryResponse>()
        .MapToApiVersion(1);
    }
}

