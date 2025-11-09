using FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

/// <summary>
/// Endpoint for updating a category.
/// </summary>
public static class UpdateCategoryEndpoint
{
    /// <summary>
    /// Maps the update category endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCategoryCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateCategoryEndpoint))
        .WithSummary("Update category")
        .WithDescription("Updates an existing category")
        .Produces<UpdateCategoryResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}
