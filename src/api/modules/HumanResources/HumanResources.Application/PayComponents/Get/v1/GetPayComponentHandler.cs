using Mapster;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;

public sealed class GetPayComponentHandler(
    [FromKeyedServices("hr:paycomponents")] IRepository<PayComponent> repository)
    : IRequestHandler<GetPayComponentRequest, PayComponentResponse>
{
    public async Task<PayComponentResponse> Handle(GetPayComponentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var payComponent = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = payComponent ?? throw new PayComponentNotFoundException(request.Id);

        return payComponent.Adapt<PayComponentResponse>();
    }
}

