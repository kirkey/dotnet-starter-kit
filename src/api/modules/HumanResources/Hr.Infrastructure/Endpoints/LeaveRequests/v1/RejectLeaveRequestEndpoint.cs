using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Reject.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for rejecting a submitted leave request.
/// Transitions a leave request from Submitted to Rejected status and releases reserved balance.
/// </summary>
public static class RejectLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapRejectLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, RejectLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectLeaveRequestEndpoint))
            .WithSummary("Rejects a leave request")
            .WithDescription("Rejects a Submitted leave request with a required reason. Request transitions to Rejected status and reserved pending balance is released per Philippines Labor Code.")
            .Produces<RejectLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}
