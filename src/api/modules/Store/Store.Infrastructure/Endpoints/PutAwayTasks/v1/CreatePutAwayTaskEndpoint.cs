using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class CreatePutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapCreatePutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePutAwayTaskCommand request, IMediator mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePutAwayTaskEndpoint))
            .WithSummary("Create a new put-away task")
            .WithDescription("Create a new put-away task")
            .Produces<CreatePutAwayTaskResponse>(200)
            .RequirePermission("store:putawaytasks:create")
            .MapToApiVersion(1);
    }
}
