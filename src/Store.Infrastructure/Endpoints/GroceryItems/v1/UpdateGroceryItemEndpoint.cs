using Store.Application.GroceryItems.Update.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.GroceryItems.v1;

public static class UpdateGroceryItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateGroceryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateGroceryItemCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateGroceryItem")
        .WithSummary("Update grocery item")
        .WithDescription("Updates an existing grocery item")
        .MapToApiVersion(1);
    }
}
