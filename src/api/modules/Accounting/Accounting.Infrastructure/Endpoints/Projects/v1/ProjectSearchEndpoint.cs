using Accounting.Application.Projects.Responses;
using Accounting.Application.Projects.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectSearchEndpoint
{
    internal static RouteHandlerBuilder MapProjectSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchProjectsCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectSearchEndpoint))
            .WithSummary("Gets a list of projects")
            .WithDescription("Gets a list of projects with pagination and filtering support")
            .Produces<PagedList<ProjectResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
