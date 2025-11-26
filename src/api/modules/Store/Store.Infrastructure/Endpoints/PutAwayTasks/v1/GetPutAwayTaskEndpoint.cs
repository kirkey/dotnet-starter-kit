using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class GetPutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapGetPutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetPutAwayTaskQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPutAwayTaskEndpoint))
            .WithSummary("Get a put-away task by ID")
            .WithDescription("Retrieves a specific put-away task with all items and details.")
            .Produces<GetPutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
