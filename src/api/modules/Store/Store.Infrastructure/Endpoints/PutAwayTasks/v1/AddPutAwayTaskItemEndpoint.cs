using FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class AddPutAwayTaskItemEndpoint
{
    internal static RouteHandlerBuilder MapAddPutAwayTaskItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/items", async (DefaultIdType id, AddPutAwayTaskItemCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AddPutAwayTaskItemEndpoint))
            .WithSummary("Add an item to a put-away task")
            .WithDescription("Adds an item to an existing put-away task for warehouse operations.")
            .Produces<AddPutAwayTaskItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
