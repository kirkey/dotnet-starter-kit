using FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class StartPickingEndpoint
{
    internal static RouteHandlerBuilder MapStartPickingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/start", async (DefaultIdType id, StartPickingCommand request, ISender sender) =>
            {
                if (id != request.PickListId)
                {
                    return Results.BadRequest("Pick list ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(StartPickingEndpoint))
            .WithSummary("Start picking")
            .WithDescription("Marks a pick list as started and records the start time.")
            .Produces<StartPickingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
