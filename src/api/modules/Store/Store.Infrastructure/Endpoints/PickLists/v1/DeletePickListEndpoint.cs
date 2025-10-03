using FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class DeletePickListEndpoint
{
    internal static RouteHandlerBuilder MapDeletePickListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var request = new DeletePickListCommand { PickListId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePickListEndpoint))
            .WithSummary("Delete a pick list")
            .WithDescription("Deletes an existing pick list.")
            .Produces<DeletePickListResponse>()
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
