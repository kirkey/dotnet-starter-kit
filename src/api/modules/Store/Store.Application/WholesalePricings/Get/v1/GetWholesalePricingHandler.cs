using Store.Domain.Exceptions.WholesalePricing;

namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Get.v1;

public sealed class GetWholesalePricingHandler(
    ILogger<GetWholesalePricingHandler> logger,
    [FromKeyedServices("store:wholesale-pricings")] IReadRepository<WholesalePricing> repository)
    : IRequestHandler<GetWholesalePricingQuery, GetWholesalePricingResponse>
{
    public async Task<GetWholesalePricingResponse> Handle(GetWholesalePricingQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetWholesalePricingSpecification(request.Id);
        var wp = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = wp ?? throw new WholesalePricingNotFoundException(request.Id);

        return wp.Adapt<GetWholesalePricingResponse>();
    }
}
