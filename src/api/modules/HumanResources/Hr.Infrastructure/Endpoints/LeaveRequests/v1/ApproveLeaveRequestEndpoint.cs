using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Approve.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for approving a submitted leave request.
/// Transitions a leave request from Submitted to Approved status and updates leave balance.
/// </summary>
public static class ApproveLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapApproveLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveLeaveRequestEndpoint))
            .WithSummary("Approves a leave request")
            .WithDescription("Approves a Submitted leave request. Manager can include optional comments. Request transitions to Approved status and leave balance is updated per Philippines Labor Code.")
            .Produces<ApproveLeaveRequestResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}

