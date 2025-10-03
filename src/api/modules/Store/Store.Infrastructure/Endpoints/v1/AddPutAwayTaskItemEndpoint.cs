using FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class AddPutAwayTaskItemEndpoint
{
    internal static RouteHandlerBuilder MapAddPutAwayTaskItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/items", async (DefaultIdType id, AddPutAwayTaskItemCommand request, IMediator mediator) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    request = request with { PutAwayTaskId = id };
                }
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AddPutAwayTaskItemEndpoint))
            .WithSummary("Add an item to a put-away task")
            .WithDescription("Add an item to a put-away task")
            .Produces<AddPutAwayTaskItemResponse>(200)
            .RequirePermission("store:putawaytasks:update")
            .MapToApiVersion(1);
    }
}
