using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Cancel.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for cancelling a leave request by the employee.
/// Can cancel Draft or Submitted requests. Approved requests cannot be cancelled.
/// </summary>
public static class CancelLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapCancelLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/cancel", async (DefaultIdType id, CancelLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelLeaveRequestEndpoint))
            .WithSummary("Cancels a leave request")
            .WithDescription("Cancels a Draft or Submitted leave request by the employee. Approved requests cannot be cancelled. Reserved pending balance is released per Philippines Labor Code.")
            .Produces<CancelLeaveRequestResponse>()
            .RequirePermission("Permissions.LeaveRequests.Cancel")
            .MapToApiVersion(1);
    }
}

