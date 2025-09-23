using Accounting.Application.Projects.Costs.Get;
using Accounting.Application.Projects.Responses;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

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
            .Produces<ProjectCostEntryResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
