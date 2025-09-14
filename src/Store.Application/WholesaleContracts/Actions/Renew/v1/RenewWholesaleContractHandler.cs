using Store.Domain.Exceptions.WholesaleContract;

namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Renew.v1;

public sealed class RenewWholesaleContractHandler(
    ILogger<RenewWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IRepository<WholesaleContract> repository)
    : IRequestHandler<RenewWholesaleContractCommand, RenewWholesaleContractResponse>
{
    public async Task<RenewWholesaleContractResponse> Handle(RenewWholesaleContractCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wc ?? throw new WholesaleContractNotFoundException(request.Id);

        wc.Renew(request.NewEndDate);
        await repository.UpdateAsync(wc, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesaleContract with id : {WholesaleContractId} renewed.", wc.Id);
        return new RenewWholesaleContractResponse(wc.Id);
    }
}

