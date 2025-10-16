namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.Specs;

/// <summary>
/// Specification to find a cycle count item by cycle count ID and item ID.
/// Used to check for duplicate items in a cycle count.
/// </summary>
public sealed class CycleCountItemByCycleCountAndItemSpec : Specification<CycleCountItem>
{
    public CycleCountItemByCycleCountAndItemSpec(DefaultIdType cycleCountId, DefaultIdType itemId)
    {
        Query.Where(x => x.CycleCountId == cycleCountId && x.ItemId == itemId);
    }
}
