using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;

public sealed class GetGroupMembershipHandler(
    [FromKeyedServices("microfinance:groupmemberships")] IRepository<GroupMembership> repository)
    : IRequestHandler<GetGroupMembershipRequest, GroupMembershipResponse>
{
    public async Task<GroupMembershipResponse> Handle(GetGroupMembershipRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var membership = await repository.FirstOrDefaultAsync(
            new GroupMembershipByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (membership is null)
        {
            throw new NotFoundException($"Group membership with ID {request.Id} not found.");
        }

        return new GroupMembershipResponse(
            membership.Id,
            membership.MemberId,
            membership.GroupId,
            membership.JoinDate,
            membership.LeaveDate,
            membership.Role,
            membership.Status,
            membership.Notes
        );
    }
}
