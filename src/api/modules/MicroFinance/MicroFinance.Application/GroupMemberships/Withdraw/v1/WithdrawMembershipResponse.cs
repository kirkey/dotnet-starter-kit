namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Withdraw.v1;

/// <summary>
/// Response after withdrawing membership.
/// </summary>
public sealed record WithdrawMembershipResponse(DefaultIdType Id, string Status, DateOnly? LeaveDate, string Message);
