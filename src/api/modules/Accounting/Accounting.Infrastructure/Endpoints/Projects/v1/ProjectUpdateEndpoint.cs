using Accounting.Application.Projects.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectUpdateEndpoint
{
    internal static RouteHandlerBuilder MapProjectUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateProjectCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectUpdateEndpoint))
            .WithSummary("update a project")
            .WithDescription("update a project")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}


