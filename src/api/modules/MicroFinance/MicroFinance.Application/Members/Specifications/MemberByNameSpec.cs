using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;

/// <summary>
/// Specification to find members by name (case-insensitive).
/// Used for search functionality.
/// </summary>
public sealed class MemberByNameSpec : Specification<Member>
{
    public MemberByNameSpec(string searchTerm)
    {
        Query.Where(m =>
            m.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            m.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            (m.MiddleName != null && m.MiddleName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
    }
}
