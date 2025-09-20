using FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class UpdateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCategoryCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateCategoryEndpoint))
        .WithSummary("Update category")
        .WithDescription("Updates an existing category")
        .Produces<UpdateCategoryResponse>()
        .RequirePermission("Permissions.Categories.Update")
        .MapToApiVersion(1);
    }
}
