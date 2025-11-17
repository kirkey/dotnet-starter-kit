using Accounting.Application.CostCenters.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterUpdateEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateCostCenterCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName(nameof(CostCenterUpdateEndpoint))
            .WithSummary("Update cost center")
            .WithDescription("Updates a cost center details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

