using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Specification to get a member by ID with projection to response.
/// </summary>
public sealed class GetMemberByIdSpec : Specification<Member, MemberResponse>, ISingleResultSpecification<Member>
{
    public GetMemberByIdSpec(DefaultIdType id)
    {
        Query.Where(m => m.Id == id);
    }
}

