namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting pending allocations.
/// </summary>
public sealed class PendingAllocationsSpec : Specification<BenefitAllocation>
{
    public PendingAllocationsSpec()
    {
        Query.Where(x => x.Status == AllocationStatus.Pending)
            .OrderBy(x => x.AllocationDate)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Employee)
            .Include(x => x.Enrollment)
                .ThenInclude(e => e.Benefit);
    }
}

