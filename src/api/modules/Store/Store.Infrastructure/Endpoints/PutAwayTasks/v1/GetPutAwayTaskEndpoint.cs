using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class GetPutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapGetPutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender sender) =>
            {
                var query = new GetPutAwayTaskQuery(id);
                var response = await sender.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPutAwayTaskEndpoint))
            .WithSummary("Get a put-away task by ID")
            .WithDescription("Retrieves a specific put-away task with all items and details.")
            .Produces<GetPutAwayTaskResponse>(200)
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
