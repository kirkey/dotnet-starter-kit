using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class GetCategoryEndpoint
{
    internal static RouteHandlerBuilder MapGetCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetCategoryCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetCategoryEndpoint))
        .WithSummary("Get category by ID")
        .WithDescription("Retrieves a category by its unique identifier")
        .Produces<CategoryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
