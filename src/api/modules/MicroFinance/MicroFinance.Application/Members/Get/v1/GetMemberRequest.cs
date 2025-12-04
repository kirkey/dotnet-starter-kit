using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;

/// <summary>
/// Request to get a member by ID.
/// </summary>
/// <param name="Id">The unique identifier of the member.</param>
public sealed record GetMemberRequest(DefaultIdType Id) : IRequest<MemberResponse>;
