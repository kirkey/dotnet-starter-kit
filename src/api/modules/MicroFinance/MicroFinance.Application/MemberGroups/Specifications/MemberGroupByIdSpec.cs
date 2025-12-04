using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;

public sealed class MemberGroupByIdSpec : Specification<MemberGroup>, ISingleResultSpecification<MemberGroup>
{
    public MemberGroupByIdSpec(Guid id)
    {
        Query.Where(mg => mg.Id == id)
            .Include(mg => mg.Memberships)
            .ThenInclude(gm => gm.Member);
    }
}
