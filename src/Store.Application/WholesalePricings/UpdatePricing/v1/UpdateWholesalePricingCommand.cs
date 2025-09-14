namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.UpdatePricing.v1;

public sealed record UpdateWholesalePricingCommand(
    DefaultIdType Id,
    decimal TierPrice,
    decimal DiscountPercentage
) : IRequest<UpdateWholesalePricingResponse>;

