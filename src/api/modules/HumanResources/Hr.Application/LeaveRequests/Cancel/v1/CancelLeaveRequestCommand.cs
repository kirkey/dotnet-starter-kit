namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Cancel.v1;

/// <summary>
/// Command to cancel a leave request (by employee).
/// Releases reserved pending balance if not yet approved.
/// </summary>
public sealed record CancelLeaveRequestCommand(
    DefaultIdType Id,
    [property: DefaultValue("No longer needed")] string Reason = ""
) : IRequest<CancelLeaveRequestResponse>;

/// <summary>
/// Response for leave request cancellation.
/// </summary>
public sealed record CancelLeaveRequestResponse(
    DefaultIdType Id,
    string Status);

