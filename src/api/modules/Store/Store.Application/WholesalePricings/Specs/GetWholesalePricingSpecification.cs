namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Specs;

public class GetWholesalePricingSpecification : Specification<WholesalePricing>
{
    public GetWholesalePricingSpecification(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

