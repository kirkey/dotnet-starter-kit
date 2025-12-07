using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.UpdateRole.v1;

/// <summary>
/// Handler for updating membership role.
/// </summary>
public sealed class UpdateMembershipRoleHandler(
    [FromKeyedServices("microfinance:groupmemberships")] IRepository<GroupMembership> repository,
    ILogger<UpdateMembershipRoleHandler> logger)
    : IRequestHandler<UpdateMembershipRoleCommand, UpdateMembershipRoleResponse>
{
    public async Task<UpdateMembershipRoleResponse> Handle(UpdateMembershipRoleCommand request, CancellationToken cancellationToken)
    {
        var membership = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Group membership with ID {request.Id} not found.");

        membership.UpdateRole(request.Role);

        await repository.UpdateAsync(membership, cancellationToken);
        logger.LogInformation("Updated role for membership {MembershipId} to {Role}", request.Id, request.Role);

        return new UpdateMembershipRoleResponse(membership.Id, membership.Role, "Membership role updated successfully.");
    }
}
