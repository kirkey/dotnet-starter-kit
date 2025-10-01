using Store.Domain.Exceptions.WholesaleContract;

namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Terminate.v1;

public sealed class TerminateWholesaleContractHandler(
    ILogger<TerminateWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IRepository<WholesaleContract> repository)
    : IRequestHandler<TerminateWholesaleContractCommand, TerminateWholesaleContractResponse>
{
    public async Task<TerminateWholesaleContractResponse> Handle(TerminateWholesaleContractCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wc ?? throw new WholesaleContractNotFoundException(request.Id);

        wc.Terminate(request.Reason);
        await repository.UpdateAsync(wc, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesaleContract with id : {WholesaleContractId} terminated.", wc.Id);
        return new TerminateWholesaleContractResponse(wc.Id);
    }
}

