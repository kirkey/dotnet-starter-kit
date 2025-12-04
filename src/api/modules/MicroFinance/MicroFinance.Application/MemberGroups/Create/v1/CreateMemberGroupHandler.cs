using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;

public sealed class CreateMemberGroupHandler(
    [FromKeyedServices("microfinance:membergroups")] IRepository<MemberGroup> repository,
    ILogger<CreateMemberGroupHandler> logger)
    : IRequestHandler<CreateMemberGroupCommand, CreateMemberGroupResponse>
{
    public async Task<CreateMemberGroupResponse> Handle(CreateMemberGroupCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate group code
        var existingGroup = await repository.FirstOrDefaultAsync(
            new MemberGroupByCodeSpec(request.Code), cancellationToken).ConfigureAwait(false);

        if (existingGroup is not null)
        {
            throw new InvalidOperationException($"A member group with code '{request.Code}' already exists.");
        }

        var group = MemberGroup.Create(
            request.Code,
            request.Name,
            request.Description,
            request.FormationDate,
            request.LeaderMemberId,
            request.LoanOfficerId,
            request.MeetingLocation,
            request.MeetingFrequency,
            request.MeetingDay,
            request.MeetingTime
        );

        await repository.AddAsync(group, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Member group {Code} created with ID {GroupId}", group.Code, group.Id);

        return new CreateMemberGroupResponse(group.Id, group.Code);
    }
}
