using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Deactivate.v1;

/// <summary>
/// Handles the DeactivateMemberGroupCommand to deactivate a member group.
/// </summary>
public sealed class DeactivateMemberGroupHandler(
    [FromKeyedServices("microfinance:membergroups")] IRepository<MemberGroup> repository,
    ILogger<DeactivateMemberGroupHandler> logger)
    : IRequestHandler<DeactivateMemberGroupCommand, DeactivateMemberGroupResponse>
{
    public async Task<DeactivateMemberGroupResponse> Handle(DeactivateMemberGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var memberGroup = await repository.GetByIdAsync(request.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(memberGroup);

        memberGroup.Deactivate();

        await repository.UpdateAsync(memberGroup, cancellationToken);

        logger.LogInformation("Member group {GroupId} deactivated", request.Id);

        return new DeactivateMemberGroupResponse(
            memberGroup.Id,
            memberGroup.Status);
    }
}
