namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Submit.v1;

/// <summary>
/// Command to submit a leave request for approval per Philippines Labor Code.
/// Validates leave balance and eligibility before submission.
/// </summary>
public sealed record SubmitLeaveRequestCommand(
    DefaultIdType Id,
    DefaultIdType ApproverManagerId
) : IRequest<SubmitLeaveRequestResponse>;

/// <summary>
/// Response for leave request submission.
/// </summary>
public sealed record SubmitLeaveRequestResponse(
    DefaultIdType Id,
    string Status,
    DateTime SubmittedDate);

