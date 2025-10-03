using FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class AddPickListItemEndpoint
{
    internal static RouteHandlerBuilder MapAddPickListItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/items", async (DefaultIdType id, AddPickListItemCommand request, ISender sender) =>
            {
                if (id != request.PickListId)
                {
                    return Results.BadRequest("Pick list ID mismatch");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AddPickListItemEndpoint))
            .WithSummary("Add item to pick list")
            .WithDescription("Adds an item to an existing pick list.")
            .Produces<AddPickListItemResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
