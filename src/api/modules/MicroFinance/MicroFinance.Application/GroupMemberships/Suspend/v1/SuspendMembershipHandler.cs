using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Suspend.v1;

/// <summary>
/// Handler for suspending membership.
/// </summary>
public sealed class SuspendMembershipHandler(
    IRepository<GroupMembership> repository,
    ILogger<SuspendMembershipHandler> logger)
    : IRequestHandler<SuspendMembershipCommand, SuspendMembershipResponse>
{
    public async Task<SuspendMembershipResponse> Handle(SuspendMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Group membership with ID {request.Id} not found.");

        membership.Suspend(request.Reason);

        await repository.UpdateAsync(membership, cancellationToken);
        logger.LogInformation("Suspended membership {MembershipId}", request.Id);

        return new SuspendMembershipResponse(membership.Id, membership.Status, "Membership suspended successfully.");
    }
}
