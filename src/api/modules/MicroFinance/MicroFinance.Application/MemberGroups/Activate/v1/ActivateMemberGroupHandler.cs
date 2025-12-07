using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Activate.v1;

/// <summary>
/// Handles the ActivateMemberGroupCommand to activate a member group.
/// </summary>
public sealed class ActivateMemberGroupHandler(
    [FromKeyedServices("microfinance:membergroups")] IRepository<MemberGroup> repository,
    ILogger<ActivateMemberGroupHandler> logger)
    : IRequestHandler<ActivateMemberGroupCommand, ActivateMemberGroupResponse>
{
    public async Task<ActivateMemberGroupResponse> Handle(ActivateMemberGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var memberGroup = await repository.GetByIdAsync(request.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(memberGroup);

        memberGroup.Activate();

        await repository.UpdateAsync(memberGroup, cancellationToken);

        logger.LogInformation("Member group {GroupId} activated", request.Id);

        return new ActivateMemberGroupResponse(
            memberGroup.Id,
            memberGroup.Status);
    }
}
