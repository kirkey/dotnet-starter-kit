using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Withdraw.v1;

/// <summary>
/// Command to withdraw a member from a group.
/// </summary>
public sealed record WithdrawMembershipCommand(DefaultIdType Id, DateOnly? LeaveDate = null, string? Reason = null) : IRequest<WithdrawMembershipResponse>;
