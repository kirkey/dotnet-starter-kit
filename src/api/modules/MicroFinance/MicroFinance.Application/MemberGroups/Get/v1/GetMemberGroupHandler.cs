using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;

/// <summary>
/// Handler for retrieving a member group by ID.
/// </summary>
public sealed class GetMemberGroupHandler(
    ILogger<GetMemberGroupHandler> logger,
    [FromKeyedServices("microfinance:membergroups")] IReadRepository<MemberGroup> repository,
    ICacheService cacheService)
    : IRequestHandler<GetMemberGroupRequest, MemberGroupResponse>
{
    public async Task<MemberGroupResponse> Handle(GetMemberGroupRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = $"membergroup:{request.Id}";

        var cached = await cacheService.GetAsync<MemberGroupResponse>(cacheKey, cancellationToken).ConfigureAwait(false);
        if (cached is not null)
        {
            return cached;
        }

        var memberGroup = await repository.FirstOrDefaultAsync(
            new MemberGroupByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (memberGroup is null)
        {
            throw new NotFoundException($"Member group with ID {request.Id} not found.");
        }

        var response = new MemberGroupResponse(
            memberGroup.Id,
            memberGroup.Code,
            memberGroup.Name,
            memberGroup.Description,
            memberGroup.FormationDate,
            memberGroup.LeaderMemberId,
            memberGroup.Leader?.FirstName != null ? $"{memberGroup.Leader.FirstName} {memberGroup.Leader.LastName}" : null,
            memberGroup.LoanOfficerId,
            memberGroup.MeetingLocation,
            memberGroup.MeetingFrequency,
            memberGroup.MeetingDay,
            memberGroup.MeetingTime,
            memberGroup.Status,
            memberGroup.Notes,
            memberGroup.Memberships?.Count ?? 0);

        await cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved member group {MemberGroupId}", request.Id);
        return response;
    }
}
