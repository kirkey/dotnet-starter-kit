using Accounting.Application.CostCenters.RecordActual.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

/// <summary>
/// Endpoint for recording cost center actual expenses.
/// </summary>
public static class CostCenterRecordActualEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterRecordActualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/actual", async (DefaultIdType id, RecordCostCenterActualCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName(nameof(CostCenterRecordActualEndpoint))
            .WithSummary("Record cost center actual expenses")
            .WithDescription("Records actual expenses/costs for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

