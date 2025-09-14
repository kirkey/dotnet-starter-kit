using Store.Domain.Exceptions.WholesaleContract;

namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Activate.v1;

public sealed class ActivateWholesaleContractHandler(
    ILogger<ActivateWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IRepository<WholesaleContract> repository)
    : IRequestHandler<ActivateWholesaleContractCommand, ActivateWholesaleContractResponse>
{
    public async Task<ActivateWholesaleContractResponse> Handle(ActivateWholesaleContractCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wc ?? throw new WholesaleContractNotFoundException(request.Id);

        wc.Activate();
        await repository.UpdateAsync(wc, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesaleContract with id : {WholesaleContractId} activated.", wc.Id);
        return new ActivateWholesaleContractResponse(wc.Id);
    }
}

