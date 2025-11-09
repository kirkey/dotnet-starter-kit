using DeferredRevenueEntity = Accounting.Domain.Entities.DeferredRevenue;

namespace Accounting.Application.DeferredRevenues.Specs;

/// <summary>
/// Specification to check for duplicate deferred revenue numbers.
/// </summary>
public sealed class DuplicateDeferredRevenueNumberSpec : Specification<DeferredRevenueEntity>
{
    public DuplicateDeferredRevenueNumberSpec(string deferredRevenueNumber)
    {
        Query.Where(d => d.DeferredRevenueNumber == deferredRevenueNumber);
    }
}

