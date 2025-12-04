using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;

public sealed class BranchByIdSpec : Specification<Branch>, ISingleResultSpecification<Branch>
{
    public BranchByIdSpec(Guid id)
    {
        Query.Where(b => b.Id == id);
    }
}
