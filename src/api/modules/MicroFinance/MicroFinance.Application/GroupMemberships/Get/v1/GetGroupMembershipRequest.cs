using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;

public sealed record GetGroupMembershipRequest(Guid Id) : IRequest<GroupMembershipResponse>;
