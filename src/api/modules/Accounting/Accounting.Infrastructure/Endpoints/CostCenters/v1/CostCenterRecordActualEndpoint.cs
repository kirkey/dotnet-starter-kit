using Accounting.Application.CostCenters.RecordActual.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterRecordActualEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterRecordActualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/actual", async (DefaultIdType id, RecordActualCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var costCenterId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = costCenterId, Message = "Actual amount recorded successfully" });
            })
            .WithName(nameof(CostCenterRecordActualEndpoint))
            .WithSummary("Record actual amount")
            .WithDescription("Records actual expenses/costs for a cost center")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

