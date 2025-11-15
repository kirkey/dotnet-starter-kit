using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests;

/// <summary>
/// Endpoint routes for managing leave requests.
/// Supports the full leave request workflow including creation, submission, approval, and rejection.
/// </summary>
public static class LeaveRequestsEndpoints
{
    internal static IEndpointRouteBuilder MapLeaveRequestsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/leave-requests")
            .WithTags("Leave Requests")
            .WithDescription("Endpoints for managing employee leave requests with Philippines Labor Code compliance including submission, approval, and rejection workflows");

        group.MapCreateLeaveRequestEndpoint();
        group.MapGetLeaveRequestEndpoint();
        group.MapUpdateLeaveRequestEndpoint();
        group.MapDeleteLeaveRequestEndpoint();
        group.MapSearchLeaveRequestsEndpoint();
        group.MapSubmitLeaveRequestEndpoint();
        group.MapApproveLeaveRequestEndpoint();
        group.MapRejectLeaveRequestEndpoint();
        group.MapCancelLeaveRequestEndpoint();

        return app;
    }
}

