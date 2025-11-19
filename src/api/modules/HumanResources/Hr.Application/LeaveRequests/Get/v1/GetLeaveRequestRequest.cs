namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;

/// <summary>
/// Request to get a leave request by its identifier.
/// </summary>
public sealed record GetLeaveRequestRequest(DefaultIdType Id) : IRequest<LeaveRequestResponse>;

