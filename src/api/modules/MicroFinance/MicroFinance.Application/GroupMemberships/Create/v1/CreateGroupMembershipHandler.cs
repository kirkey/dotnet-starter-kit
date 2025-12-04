using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Create.v1;

public sealed class CreateGroupMembershipHandler(
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:membergroups")] IReadRepository<MemberGroup> groupRepository,
    [FromKeyedServices("microfinance:groupmemberships")] IRepository<GroupMembership> membershipRepository,
    ILogger<CreateGroupMembershipHandler> logger)
    : IRequestHandler<CreateGroupMembershipCommand, CreateGroupMembershipResponse>
{
    public async Task<CreateGroupMembershipResponse> Handle(CreateGroupMembershipCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify member exists
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        // Verify group exists
        var group = await groupRepository.FirstOrDefaultAsync(
            new MemberGroupByIdSpec(request.GroupId), cancellationToken).ConfigureAwait(false);

        if (group is null)
        {
            throw new NotFoundException($"Group with ID {request.GroupId} not found.");
        }

        // Create the membership
        var membership = GroupMembership.Create(
            request.MemberId,
            request.GroupId,
            request.JoinDate,
            request.Role,
            request.Notes);

        await membershipRepository.AddAsync(membership, cancellationToken).ConfigureAwait(false);
        await membershipRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Created group membership {MembershipId} for member {MemberId} in group {GroupId}",
            membership.Id, request.MemberId, request.GroupId);

        return new CreateGroupMembershipResponse(membership.Id);
    }
}
