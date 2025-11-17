using Accounting.Application.Projects.Costing.Get;
using Accounting.Application.Projects.Costing.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

public static class ProjectCostingGetEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostingGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectCostingQuery(id));
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithName(nameof(ProjectCostingGetEndpoint))
            .WithSummary("get project costing entry")
            .WithDescription("Gets a single project costing entry by ID")
            .Produces<ProjectCostingResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
