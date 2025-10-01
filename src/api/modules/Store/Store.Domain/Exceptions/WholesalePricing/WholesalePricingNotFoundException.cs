namespace Store.Domain.Exceptions.WholesalePricing;

public sealed class WholesalePricingNotFoundException(DefaultIdType id)
    : NotFoundException($"WholesalePricing with ID {id} was not found.");

