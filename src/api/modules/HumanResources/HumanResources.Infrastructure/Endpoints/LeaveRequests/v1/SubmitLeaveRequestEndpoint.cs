using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Submit.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for submitting a leave request for approval.
/// Transitions a leave request from Draft to Submitted status.
/// </summary>
public static class SubmitLeaveRequestEndpoint
{
    internal static RouteHandlerBuilder MapSubmitLeaveRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/submit", async (DefaultIdType id, SubmitLeaveRequestCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Accepted($"/leave-requests/{response.Id}", response);
            })
            .WithName(nameof(SubmitLeaveRequestEndpoint))
            .WithSummary("Submits a leave request for approval")
            .WithDescription("Submits a Draft leave request for manager approval. Validates leave balance and eligibility per Philippines Labor Code. Request transitions to Submitted status.")
            .Produces<SubmitLeaveRequestResponse>(StatusCodes.Status202Accepted)
            .RequirePermission("Permissions.LeaveRequests.Submit")
            .MapToApiVersion(1);
    }
}

