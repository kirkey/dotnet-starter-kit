using Mapster;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;

public sealed class GetPayComponentRateHandler(
    [FromKeyedServices("humanresources:paycomponentrates")] IRepository<PayComponentRate> repository)
    : IRequestHandler<GetPayComponentRateRequest, PayComponentRateResponse>
{
    public async Task<PayComponentRateResponse> Handle(GetPayComponentRateRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var rate = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = rate ?? throw new PayComponentRateNotFoundException(request.Id);

        return rate.Adapt<PayComponentRateResponse>();
    }
}

