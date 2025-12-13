using FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class AssignPickListEndpoint
{
    internal static RouteHandlerBuilder MapAssignPickListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/assign", async (DefaultIdType id, AssignPickListCommand request, ISender sender) =>
            {
                if (id != request.PickListId)
                {
                    return Results.BadRequest("Pick list ID mismatch");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AssignPickListEndpoint))
            .WithSummary("Assign pick list to picker")
            .WithDescription("Assigns a pick list to a warehouse picker.")
            .Produces<AssignPickListResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
