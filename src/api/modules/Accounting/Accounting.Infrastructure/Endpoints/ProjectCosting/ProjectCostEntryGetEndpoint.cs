using Accounting.Application.ProjectCosting.Get;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting;

public static class ProjectCostEntryGetEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostEntryGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{projectId:guid}/costs/{entryId:guid}", async (DefaultIdType projectId, DefaultIdType entryId, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectCostEntryQuery(projectId, entryId)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectCostEntryGetEndpoint))
            .WithSummary("get a project cost entry")
            .WithDescription("get a project cost entry by id for a project")
            .Produces<ProjectCostResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
