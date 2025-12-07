using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dissolve.v1;

/// <summary>
/// Handles the DissolveMemberGroupCommand to dissolve a member group.
/// </summary>
public sealed class DissolveMemberGroupHandler(
    [FromKeyedServices("microfinance:membergroups")] IRepository<MemberGroup> repository,
    ILogger<DissolveMemberGroupHandler> logger)
    : IRequestHandler<DissolveMemberGroupCommand, DissolveMemberGroupResponse>
{
    public async Task<DissolveMemberGroupResponse> Handle(DissolveMemberGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var memberGroup = await repository.GetByIdAsync(request.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(memberGroup);

        memberGroup.Dissolve(request.Reason);

        await repository.UpdateAsync(memberGroup, cancellationToken);

        logger.LogInformation("Member group {GroupId} dissolved. Reason: {Reason}", request.Id, request.Reason ?? "Not specified");

        return new DissolveMemberGroupResponse(
            memberGroup.Id,
            memberGroup.Status);
    }
}
