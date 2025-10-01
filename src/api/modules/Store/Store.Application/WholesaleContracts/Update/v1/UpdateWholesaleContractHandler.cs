using Store.Domain.Exceptions.WholesaleContract;

namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Update.v1;

public sealed class UpdateWholesaleContractHandler(
    ILogger<UpdateWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IRepository<WholesaleContract> repository)
    : IRequestHandler<UpdateWholesaleContractCommand, UpdateWholesaleContractResponse>
{
    public async Task<UpdateWholesaleContractResponse> Handle(UpdateWholesaleContractCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wc = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = wc ?? throw new WholesaleContractNotFoundException(request.Id);

        var updated = wc.Update(
            request.StartDate,
            request.EndDate,
            request.MinimumOrderValue,
            request.VolumeDiscountPercentage,
            request.PaymentTermsDays,
            request.CreditLimit,
            request.DeliveryTerms,
            request.ContractTerms,
            request.AutoRenewal,
            request.Notes,
            request.Status);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("WholesaleContract with id : {WholesaleContractId} updated.", wc.Id);
        return new UpdateWholesaleContractResponse(wc.Id);
    }
}

