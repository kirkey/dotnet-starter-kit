using Accounting.Application.Projects.Costing.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

public static class ProjectCostingUpdateEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostingUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateProjectCostingCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(request);
                return Results.NoContent();
            })
            .WithName(nameof(ProjectCostingUpdateEndpoint))
            .WithSummary("update project costing entry")
            .WithDescription("Updates an existing project costing entry")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
