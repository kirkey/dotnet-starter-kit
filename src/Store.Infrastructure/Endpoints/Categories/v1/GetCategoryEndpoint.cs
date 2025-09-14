using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class GetCategoryEndpoint
{
    internal static RouteHandlerBuilder MapGetCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetCategory")
        .WithSummary("Get category by ID")
        .WithDescription("Retrieves a category by its unique identifier")
        .MapToApiVersion(1);
    }
}

