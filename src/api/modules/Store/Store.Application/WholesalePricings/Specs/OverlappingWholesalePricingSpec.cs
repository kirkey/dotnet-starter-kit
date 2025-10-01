namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Specs;

public sealed class OverlappingWholesalePricingSpec : Specification<WholesalePricing>, ISingleResultSpecification<WholesalePricing>
{
    public OverlappingWholesalePricingSpec(
        DefaultIdType wholesaleContractId,
        DefaultIdType groceryItemId,
        int minimumQuantity,
        int? maximumQuantity,
        DateTime effectiveDate,
        DateTime? expiryDate)
    {
        // Overlap logic:
        // same contract & grocery item
        // AND quantities overlap: (existing.Min <= new.Max OR existing.Max is null) AND (existing.Max >= new.Min OR existing.Max is null)
        // AND dates overlap: (existing.EffectiveDate <= new.Expiry OR new.Expiry is null) AND (existing.ExpiryDate >= new.EffectiveDate OR existing.ExpiryDate is null)

        Query.Where(x => x.WholesaleContractId == wholesaleContractId && x.GroceryItemId == groceryItemId);

        Query.Where(x =>
            // quantity overlap
            (x.MaximumQuantity == null || maximumQuantity == null ? true : x.MaximumQuantity >= minimumQuantity) &&
            (maximumQuantity == null || x.MinimumQuantity <= maximumQuantity.Value)
        );

        Query.Where(x =>
            // date overlap
            (x.ExpiryDate == null || expiryDate == null ? true : x.ExpiryDate >= effectiveDate) &&
            (expiryDate == null || x.EffectiveDate <= expiryDate.Value)
        );
    }
}

