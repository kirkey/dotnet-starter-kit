using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Request to get a member by ID.
/// </summary>
public sealed record GetMemberRequest(DefaultIdType Id) : IRequest<MemberResponse>;

