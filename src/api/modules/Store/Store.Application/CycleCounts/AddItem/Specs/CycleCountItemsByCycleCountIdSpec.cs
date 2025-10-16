namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.Specs;

/// <summary>
/// Specification to retrieve all cycle count items for a specific cycle count.
/// Used to recalculate totals after adding/updating/deleting items.
/// </summary>
public sealed class CycleCountItemsByCycleCountIdSpec : Specification<CycleCountItem>
{
    public CycleCountItemsByCycleCountIdSpec(DefaultIdType cycleCountId)
    {
        Query.Where(x => x.CycleCountId == cycleCountId);
    }
}
