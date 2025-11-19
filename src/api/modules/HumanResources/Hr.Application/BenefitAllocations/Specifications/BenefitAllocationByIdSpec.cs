namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Specifications;

using Ardalis.Specification;
using Domain.Entities;

/// <summary>
/// Specification for getting benefit allocation by ID.
/// </summary>
public sealed class BenefitAllocationByIdSpec : Specification<BenefitAllocation>, ISingleResultSpecification<BenefitAllocation>
{
    public BenefitAllocationByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Employee)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Benefit);
    }
}

