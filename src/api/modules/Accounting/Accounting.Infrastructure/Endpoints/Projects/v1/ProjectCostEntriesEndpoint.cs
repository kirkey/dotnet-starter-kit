using Accounting.Application.ProjectCosting.Get;
using Accounting.Application.Projects.Responses;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectCostEntriesEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostEntriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{projectId:guid}/costs", async (DefaultIdType projectId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectCostEntriesQuery(projectId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MapProjectCostEntriesEndpoint))
            .WithSummary("list project cost entries")
            .WithDescription("list all project cost entries for a project")
            .Produces<ICollection<ProjectCostResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
