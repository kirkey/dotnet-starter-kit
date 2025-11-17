using FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class CompletePickingEndpoint
{
    internal static RouteHandlerBuilder MapCompletePickingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (DefaultIdType id, CompletePickingCommand request, ISender sender) =>
            {
                if (id != request.PickListId)
                {
                    return Results.BadRequest("Pick list ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePickingEndpoint))
            .WithSummary("Complete picking")
            .WithDescription("Marks a pick list as completed and records the completion time.")
            .Produces<CompletePickingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
