using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Specifications;

public sealed class MemberGroupByCodeSpec : Specification<MemberGroup>, ISingleResultSpecification<MemberGroup>
{
    public MemberGroupByCodeSpec(string code)
    {
        Query.Where(mg => mg.Code == code);
    }
}
