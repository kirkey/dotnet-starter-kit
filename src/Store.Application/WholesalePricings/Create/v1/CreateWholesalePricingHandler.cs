namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Create.v1;

public sealed class CreateWholesalePricingHandler(
    ILogger<CreateWholesalePricingHandler> logger,
    [FromKeyedServices("store:wholesale-pricings")] IRepository<WholesalePricing> repository)
    : IRequestHandler<CreateWholesalePricingCommand, CreateWholesalePricingResponse>
{
    public async Task<CreateWholesalePricingResponse> Handle(CreateWholesalePricingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pricing = WholesalePricing.Create(
            request.WholesaleContractId,
            request.GroceryItemId,
            request.MinimumQuantity,
            request.MaximumQuantity,
            request.TierPrice,
            request.DiscountPercentage,
            request.EffectiveDate,
            request.ExpiryDate,
            request.Notes);

        await repository.AddAsync(pricing, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("WholesalePricing created {WholesalePricingId}", pricing.Id);
        return new CreateWholesalePricingResponse(pricing.Id);
    }
}

