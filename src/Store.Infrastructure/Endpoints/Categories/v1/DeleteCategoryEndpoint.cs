using FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class DeleteCategoryEndpoint
{
    internal static RouteHandlerBuilder MapDeleteCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteCategoryCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteCategoryEndpoint))
        .WithSummary("Delete category")
        .WithDescription("Deletes a category by its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.Categories.Delete")
        .MapToApiVersion(1);
    }
}
