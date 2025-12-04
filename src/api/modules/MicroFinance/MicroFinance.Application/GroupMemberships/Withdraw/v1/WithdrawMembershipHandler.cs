using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Withdraw.v1;

/// <summary>
/// Handler for withdrawing membership.
/// </summary>
public sealed class WithdrawMembershipHandler(
    IRepository<GroupMembership> repository,
    ILogger<WithdrawMembershipHandler> logger)
    : IRequestHandler<WithdrawMembershipCommand, WithdrawMembershipResponse>
{
    public async Task<WithdrawMembershipResponse> Handle(WithdrawMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Group membership with ID {request.Id} not found.");

        membership.Withdraw(request.LeaveDate, request.Reason);

        await repository.UpdateAsync(membership, cancellationToken);
        logger.LogInformation("Withdrawn membership {MembershipId}", request.Id);

        return new WithdrawMembershipResponse(membership.Id, membership.Status, membership.LeaveDate, "Membership withdrawn successfully.");
    }
}
