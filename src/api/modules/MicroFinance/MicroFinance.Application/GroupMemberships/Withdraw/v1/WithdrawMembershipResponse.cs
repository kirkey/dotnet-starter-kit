namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Withdraw.v1;

/// <summary>
/// Response after withdrawing membership.
/// </summary>
public sealed record WithdrawMembershipResponse(Guid Id, string Status, DateOnly? LeaveDate, string Message);
