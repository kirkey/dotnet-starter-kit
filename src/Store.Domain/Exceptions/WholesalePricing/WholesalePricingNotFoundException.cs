namespace Store.Domain.Exceptions.WholesalePricing;

public sealed class WholesalePricingNotFoundException : NotFoundException
{
    public WholesalePricingNotFoundException(DefaultIdType id) : base($"WholesalePricing with ID {id} was not found.") { }
}

