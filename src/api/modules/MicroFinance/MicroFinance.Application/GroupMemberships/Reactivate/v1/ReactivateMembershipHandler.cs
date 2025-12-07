using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Reactivate.v1;

/// <summary>
/// Handler for reactivating membership.
/// </summary>
public sealed class ReactivateMembershipHandler(
    [FromKeyedServices("microfinance:groupmemberships")] IRepository<GroupMembership> repository,
    ILogger<ReactivateMembershipHandler> logger)
    : IRequestHandler<ReactivateMembershipCommand, ReactivateMembershipResponse>
{
    public async Task<ReactivateMembershipResponse> Handle(ReactivateMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Group membership with ID {request.Id} not found.");

        membership.Reactivate();

        await repository.UpdateAsync(membership, cancellationToken);
        logger.LogInformation("Reactivated membership {MembershipId}", request.Id);

        return new ReactivateMembershipResponse(membership.Id, membership.Status, "Membership reactivated successfully.");
    }
}
