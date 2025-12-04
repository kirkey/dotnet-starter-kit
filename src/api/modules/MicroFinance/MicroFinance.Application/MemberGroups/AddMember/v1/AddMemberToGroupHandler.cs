using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;

public sealed class AddMemberToGroupHandler(
    [FromKeyedServices("microfinance:membergroups")] IReadRepository<MemberGroup> groupRepository,
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:groupmemberships")] IRepository<GroupMembership> membershipRepository,
    ILogger<AddMemberToGroupHandler> logger)
    : IRequestHandler<AddMemberToGroupCommand, AddMemberToGroupResponse>
{
    public async Task<AddMemberToGroupResponse> Handle(AddMemberToGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify group exists
        var group = await groupRepository.FirstOrDefaultAsync(
            new MemberGroupByIdSpec(request.GroupId), cancellationToken).ConfigureAwait(false);

        if (group is null)
        {
            throw new NotFoundException($"Group with ID {request.GroupId} not found.");
        }

        // Verify member exists
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        // Create the membership
        var membership = GroupMembership.Create(
            request.MemberId,
            request.GroupId,
            request.JoinDate,
            request.Role ?? GroupMembership.RoleMember,
            request.Notes);

        await membershipRepository.AddAsync(membership, cancellationToken).ConfigureAwait(false);
        await membershipRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Added member {MemberId} to group {GroupId} with role {Role}. Membership ID: {MembershipId}",
            request.MemberId, request.GroupId, membership.Role, membership.Id);

        return new AddMemberToGroupResponse(
            membership.Id,
            request.GroupId,
            request.MemberId,
            membership.Role);
    }
}
