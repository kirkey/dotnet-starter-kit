using Store.Domain.Exceptions.WholesalePricing;

namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Deactivate.v1;

public sealed class DeactivateWholesalePricingHandler(
    ILogger<DeactivateWholesalePricingHandler> logger,
    [FromKeyedServices("store:wholesale-pricings")] IRepository<WholesalePricing> repository)
    : IRequestHandler<DeactivateWholesalePricingCommand, DeactivateWholesalePricingResponse>
{
    public async Task<DeactivateWholesalePricingResponse> Handle(DeactivateWholesalePricingCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wp = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wp ?? throw new WholesalePricingNotFoundException(request.Id);

        wp.Deactivate();
        await repository.UpdateAsync(wp, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesalePricing with id : {WholesalePricingId} deactivated.", wp.Id);
        return new DeactivateWholesalePricingResponse(wp.Id);
    }
}
