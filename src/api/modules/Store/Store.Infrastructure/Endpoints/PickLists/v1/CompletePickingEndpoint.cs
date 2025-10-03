using FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class CompletePickingEndpoint
{
    internal static RouteHandlerBuilder MapCompletePickingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (DefaultIdType id, ISender sender) =>
            {
                var request = new CompletePickingCommand { PickListId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePickingEndpoint))
            .WithSummary("Complete picking")
            .WithDescription("Marks a pick list as completed and records the completion time.")
            .Produces<CompletePickingResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
