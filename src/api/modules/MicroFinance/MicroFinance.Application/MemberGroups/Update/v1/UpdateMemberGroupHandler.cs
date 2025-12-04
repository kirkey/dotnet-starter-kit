using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

/// <summary>
/// Handles the UpdateMemberGroupCommand to update a member group's information.
/// </summary>
public sealed class UpdateMemberGroupHandler(
    IRepository<MemberGroup> repository,
    ILogger<UpdateMemberGroupHandler> logger)
    : IRequestHandler<UpdateMemberGroupCommand, UpdateMemberGroupResponse>
{
    public async Task<UpdateMemberGroupResponse> Handle(UpdateMemberGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var memberGroup = await repository.GetByIdAsync(request.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(memberGroup);

        memberGroup.Update(
            request.Name,
            request.Description,
            request.LeaderMemberId,
            request.LoanOfficerId,
            request.MeetingLocation,
            request.MeetingFrequency,
            request.MeetingDay,
            request.MeetingTime,
            request.Notes);

        await repository.UpdateAsync(memberGroup, cancellationToken);

        logger.LogInformation("Member group {GroupId} updated", request.Id);

        return new UpdateMemberGroupResponse(
            memberGroup.Id,
            memberGroup.Name,
            memberGroup.Status);
    }
}
