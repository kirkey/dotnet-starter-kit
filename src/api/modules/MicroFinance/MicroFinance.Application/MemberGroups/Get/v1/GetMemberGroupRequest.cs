using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;

/// <summary>
/// Request to retrieve a member group by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the member group.</param>
public record GetMemberGroupRequest(Guid Id) : IRequest<MemberGroupResponse>;
