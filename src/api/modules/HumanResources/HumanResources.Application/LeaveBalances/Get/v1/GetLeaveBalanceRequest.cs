namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Get.v1;

/// <summary>
/// Request to get a leave balance by its identifier.
/// </summary>
public sealed record GetLeaveBalanceRequest(DefaultIdType Id) : IRequest<LeaveBalanceResponse>;

