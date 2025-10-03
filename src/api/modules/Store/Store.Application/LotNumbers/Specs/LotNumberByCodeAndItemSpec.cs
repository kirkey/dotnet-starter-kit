namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Specs;

/// <summary>
/// Specification to find a lot number by lot code and item ID.
/// </summary>
public sealed class LotNumberByCodeAndItemSpec : Specification<LotNumber>
{
    public LotNumberByCodeAndItemSpec(string lotCode, DefaultIdType itemId)
    {
        Query.Where(l => l.LotCode == lotCode && l.ItemId == itemId);
    }
}
