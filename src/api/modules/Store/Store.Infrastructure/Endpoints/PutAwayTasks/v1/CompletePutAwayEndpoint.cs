using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class CompletePutAwayEndpoint
{
    internal static RouteHandlerBuilder MapCompletePutAwayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, CompletePutAwayCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePutAwayEndpoint))
            .WithSummary("Complete a put-away task")
            .WithDescription("Marks a put-away task as completed and records the completion time.")
            .Produces<CompletePutAwayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
