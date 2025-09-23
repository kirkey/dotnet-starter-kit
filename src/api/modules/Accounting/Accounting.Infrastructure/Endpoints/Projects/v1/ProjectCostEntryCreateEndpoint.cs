using Accounting.Application.Projects.Costs.Create;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectCostEntryCreateEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostEntryCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{projectId:guid}/costs", async (DefaultIdType projectId, CreateProjectCostEntryCommand body, ISender mediator) =>
            {
                if (projectId != body.ProjectId) return Results.BadRequest();
                var id = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(ProjectCostEntryCreateEndpoint))
            .WithSummary("create a project cost entry")
            .WithDescription("create a project cost entry for a project")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
