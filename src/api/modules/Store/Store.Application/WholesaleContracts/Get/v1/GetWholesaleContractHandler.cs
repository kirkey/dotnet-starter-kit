using Store.Domain.Exceptions.WholesaleContract;

namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Get.v1;

public sealed class GetWholesaleContractHandler(
    ILogger<GetWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IReadRepository<WholesaleContract> repository)
    : IRequestHandler<GetWholesaleContractQuery, GetWholesaleContractResponse>
{
    public async Task<GetWholesaleContractResponse> Handle(GetWholesaleContractQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetWholesaleContractSpecification(request.Id);
        var wc = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = wc ?? throw new WholesaleContractNotFoundException(request.Id);

        return wc.Adapt<GetWholesaleContractResponse>();
    }
}
