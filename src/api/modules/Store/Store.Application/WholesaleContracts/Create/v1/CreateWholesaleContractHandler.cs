namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Create.v1;

public sealed class CreateWholesaleContractHandler(
    ILogger<CreateWholesaleContractHandler> logger,
    [FromKeyedServices("store:wholesale-contracts")] IRepository<WholesaleContract> repository)
    : IRequestHandler<CreateWholesaleContractCommand, CreateWholesaleContractResponse>
{
    public async Task<CreateWholesaleContractResponse> Handle(CreateWholesaleContractCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wholesaleContract = WholesaleContract.Create(
            request.ContractNumber,
            request.CustomerId,
            request.StartDate,
            request.EndDate,
            request.MinimumOrderValue,
            request.VolumeDiscountPercentage,
            request.PaymentTermsDays,
            request.CreditLimit,
            request.DeliveryTerms,
            request.ContractTerms,
            request.AutoRenewal,
            request.Notes);

        await repository.AddAsync(wholesaleContract, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("WholesaleContract created {WholesaleContractId}", wholesaleContract.Id);
        return new CreateWholesaleContractResponse(wholesaleContract.Id);
    }
}

