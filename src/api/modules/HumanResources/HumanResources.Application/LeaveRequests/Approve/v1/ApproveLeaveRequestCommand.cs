namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Approve.v1;

/// <summary>
/// Command to approve a leave request per Philippines Labor Code.
/// Converts pending balance to taken.
/// </summary>
public sealed record ApproveLeaveRequestCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Comment = null
) : IRequest<ApproveLeaveRequestResponse>;

/// <summary>
/// Response for leave request approval.
/// </summary>
public sealed record ApproveLeaveRequestResponse(
    DefaultIdType Id,
    string Status,
    DateTime ReviewedDate);

