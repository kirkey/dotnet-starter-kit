using FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;
using Shared.Authorization;

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
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);
    }
}
