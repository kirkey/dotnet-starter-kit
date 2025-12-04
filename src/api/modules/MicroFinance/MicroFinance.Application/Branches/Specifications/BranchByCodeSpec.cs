using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;

public sealed class BranchByCodeSpec : Specification<Branch>, ISingleResultSpecification<Branch>
{
    public BranchByCodeSpec(string code)
    {
        Query.Where(b => b.Code == code);
    }
}
