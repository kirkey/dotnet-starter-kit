using Accounting.Application.CostCenters.Get;
using Accounting.Application.CostCenters.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterGetEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCostCenterRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CostCenterGetEndpoint))
            .WithSummary("Get cost center by ID")
            .WithDescription("Retrieves a cost center by its unique identifier")
            .Produces<CostCenterResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

