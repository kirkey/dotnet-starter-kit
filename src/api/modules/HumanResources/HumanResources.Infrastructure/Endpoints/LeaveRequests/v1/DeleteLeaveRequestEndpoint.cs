using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for deleting a leave request.
/// Only Draft or Rejected requests can be deleted per Philippines Labor Code compliance.
/// </summary>
public static class DeleteLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapDeleteLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeleteLeaveRequestCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteLeaveRequestEndpoint))
            .WithSummary("Deletes a leave request")
            .WithDescription("Deletes a leave request. Only Draft and Rejected requests can be deleted. Approved or Submitted requests cannot be deleted without proper workflow steps.")
            .Produces<DeleteLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}
