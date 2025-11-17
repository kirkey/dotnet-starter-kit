using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class StartPutAwayEndpoint
{
    internal static RouteHandlerBuilder MapStartPutAwayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/start", async (DefaultIdType id, StartPutAwayCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(StartPutAwayEndpoint))
            .WithSummary("Start a put-away task")
            .WithDescription("Marks a put-away task as started and records the start time.")
            .Produces<StartPutAwayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
