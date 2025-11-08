using Accounting.Application.CostCenters.RecordActual.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterRecordActualEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterRecordActualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/actual", async (DefaultIdType id, RecordActualCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId });
            })
            .WithName(nameof(CostCenterRecordActualEndpoint))
            .WithSummary("record actual expenses")
            .WithDescription("records actual expenses/costs for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

