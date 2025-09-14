using FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class UpdateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCategoryCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateCategory")
        .WithSummary("Update category")
        .WithDescription("Updates an existing category")
        .MapToApiVersion(1);
    }
}

