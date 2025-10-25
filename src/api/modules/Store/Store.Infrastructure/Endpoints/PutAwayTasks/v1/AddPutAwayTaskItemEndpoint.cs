using FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

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
            .Produces<AddPutAwayTaskItemResponse>(200)
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
