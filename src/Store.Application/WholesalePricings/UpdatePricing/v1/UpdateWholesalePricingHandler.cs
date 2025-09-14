using Store.Domain.Exceptions.WholesalePricing;

namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.UpdatePricing.v1;

public sealed class UpdateWholesalePricingHandler(
    ILogger<UpdateWholesalePricingHandler> logger,
    [FromKeyedServices("store:wholesale-pricings")] IRepository<WholesalePricing> repository)
    : IRequestHandler<UpdateWholesalePricingCommand, UpdateWholesalePricingResponse>
{
    public async Task<UpdateWholesalePricingResponse> Handle(UpdateWholesalePricingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wp = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wp ?? throw new WholesalePricingNotFoundException(request.Id);

        wp.UpdatePricing(request.TierPrice, request.DiscountPercentage);
        await repository.UpdateAsync(wp, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesalePricing with id : {WholesalePricingId} updated.", wp.Id);
        return new UpdateWholesalePricingResponse(wp.Id);
    }
}
