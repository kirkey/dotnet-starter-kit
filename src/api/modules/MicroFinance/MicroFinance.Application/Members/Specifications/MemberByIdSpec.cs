using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;

/// <summary>
/// Specification to find a member by ID.
/// </summary>
public sealed class MemberByIdSpec : Specification<Member>
{
    public MemberByIdSpec(DefaultIdType id)
    {
        Query.Where(m => m.Id == id);
    }
}
