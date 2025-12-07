using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Request to get a utility member by ID.
/// </summary>
public sealed record GetUtilityMemberRequest(DefaultIdType Id) : IRequest<UtilityMemberResponse>;

