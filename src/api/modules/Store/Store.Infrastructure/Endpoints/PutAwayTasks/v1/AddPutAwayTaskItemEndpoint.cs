using FSH.Starter.WebApi.Store.Application.PutAwayTasks.AddItem.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class AddPutAwayTaskItemEndpoint
{
    internal static RouteHandlerBuilder MapAddPutAwayTaskItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/items", async (DefaultIdType id, AddPutAwayTaskItemRequestDto request, IMediator mediator) =>
            {
                var command = new AddPutAwayTaskItemCommand(
                    id,
                    request.ItemId,
                    request.ToBinId,
                    request.LotNumberId,
                    request.SerialNumberId,
                    request.Quantity,
                    request.Notes
                );
                var response = await mediator.Send(command).ConfigureAwait(false);
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
