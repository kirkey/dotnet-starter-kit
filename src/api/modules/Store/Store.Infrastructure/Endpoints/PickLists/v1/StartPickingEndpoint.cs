using FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;

namespace Store.Infrastructure.Endpoints.PickLists.v1;

public static class StartPickingEndpoint
{
    internal static RouteHandlerBuilder MapStartPickingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/start", async (DefaultIdType id, ISender sender) =>
            {
                var request = new StartPickingCommand { PickListId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(StartPickingEndpoint))
            .WithSummary("Start picking")
            .WithDescription("Marks a pick list as started and records the start time.")
            .Produces<StartPickingResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
