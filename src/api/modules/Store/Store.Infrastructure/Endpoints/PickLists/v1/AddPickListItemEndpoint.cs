using FSH.Starter.WebApi.Store.Application.PickLists.AddItem.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

/// <summary>
/// Endpoint for adding an item to a pick list.
/// Creates PickListItem as a separate aggregate and updates parent PickList totals.
/// </summary>
public static class AddPickListItemEndpoint
{
    internal static RouteHandlerBuilder MapAddPickListItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/items", async (DefaultIdType id, AddPickListItemCommand request, ISender sender) =>
            {
                if (id != request.PickListId)
                {
                    return Results.BadRequest("Pick list ID in URL does not match request body");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AddPickListItemEndpoint))
            .WithSummary("Add item to pick list")
            .WithDescription("Adds an item to an existing pick list. The pick list must be in 'Created' status. Creates PickListItem as a separate aggregate.")
            .Produces<AddPickListItemResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
