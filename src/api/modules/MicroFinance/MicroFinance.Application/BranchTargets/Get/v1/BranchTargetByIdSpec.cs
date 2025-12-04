using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;

/// <summary>
/// Specification to get a branch target by ID.
/// </summary>
public sealed class BranchTargetByIdSpec : Specification<BranchTarget>, ISingleResultSpecification<BranchTarget>
{
    public BranchTargetByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
