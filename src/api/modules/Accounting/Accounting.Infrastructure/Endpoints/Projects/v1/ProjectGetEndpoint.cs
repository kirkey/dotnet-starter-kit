using Accounting.Application.Projects.Get.v1;
using Accounting.Application.Projects.Responses;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectGetEndpoint
{
    internal static RouteHandlerBuilder MapProjectGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectGetEndpoint))
            .WithSummary("get a project by id")
            .WithDescription("get a project by id")
            .Produces<ProjectResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


