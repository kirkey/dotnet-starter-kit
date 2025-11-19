namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Reject.v1;

/// <summary>
/// Command to reject a leave request per Philippines Labor Code.
/// Releases reserved pending balance.
/// </summary>
public sealed record RejectLeaveRequestCommand(
    DefaultIdType Id,
    [property: DefaultValue("Insufficient balance")] string Reason
) : IRequest<RejectLeaveRequestResponse>;

/// <summary>
/// Response for leave request rejection.
/// </summary>
public sealed record RejectLeaveRequestResponse(
    DefaultIdType Id,
    string Status,
    DateTime ReviewedDate,
    string Reason);

