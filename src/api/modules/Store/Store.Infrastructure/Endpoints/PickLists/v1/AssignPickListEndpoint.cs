using FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class AssignPickListEndpoint
{
    internal static RouteHandlerBuilder MapAssignPickListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/assign", async (DefaultIdType id, AssignPickListCommand request, ISender sender) =>
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
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
