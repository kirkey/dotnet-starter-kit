using Accounting.Application.Members.Responses;

namespace Accounting.Application.Members.Get.v1;

/// <summary>
/// Specification to get a utility member by ID with projection to response.
/// </summary>
public sealed class GetUtilityMemberByIdSpec : Specification<Member, UtilityMemberResponse>, ISingleResultSpecification<Member>
{
    public GetUtilityMemberByIdSpec(DefaultIdType id)
    {
        Query.Where(m => m.Id == id);
    }
}

