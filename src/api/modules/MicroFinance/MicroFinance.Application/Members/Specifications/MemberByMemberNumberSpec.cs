using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;

/// <summary>
/// Specification to find a member by member number (case-insensitive).
/// Used for uniqueness validation.
/// </summary>
public sealed class MemberByMemberNumberSpec : Specification<Member>
{
    public MemberByMemberNumberSpec(string memberNumber)
    {
        Query.Where(m => m.MemberNumber.Equals(memberNumber, StringComparison.OrdinalIgnoreCase));
    }
}
