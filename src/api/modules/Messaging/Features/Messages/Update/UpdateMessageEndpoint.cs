namespace FSH.Starter.WebApi.Messaging.Features.Messages.Update;

public static class UpdateMessageEndpoint
{
    internal static RouteHandlerBuilder MapUpdateMessageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateMessageCommand request, ISender mediator) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("id in route does not match id in request body");
            }

            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateMessageEndpoint))
        .WithSummary("updates a message")
        .WithDescription("updates message content")
        .Produces<UpdateMessageResponse>()
        .RequirePermission("Permissions.Messaging.Update")
        .MapToApiVersion(1);
    }
}

