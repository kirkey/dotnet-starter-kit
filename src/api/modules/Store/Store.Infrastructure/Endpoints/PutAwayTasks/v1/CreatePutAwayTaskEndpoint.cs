using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Create.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class CreatePutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapCreatePutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePutAwayTaskCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePutAwayTaskEndpoint))
            .WithSummary("Create a new put-away task")
            .WithDescription("Creates a new put-away task for warehouse operations.")
            .Produces<CreatePutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
