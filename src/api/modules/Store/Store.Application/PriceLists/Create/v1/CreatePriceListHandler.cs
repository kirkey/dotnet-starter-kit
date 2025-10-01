namespace FSH.Starter.WebApi.Store.Application.PriceLists.Create.v1;

public sealed class CreatePriceListHandler(
    ILogger<CreatePriceListHandler> logger,
    [FromKeyedServices("store:price-lists")] IRepository<PriceList> repository)
    : IRequestHandler<CreatePriceListCommand, CreatePriceListResponse>
{
    public async Task<CreatePriceListResponse> Handle(CreatePriceListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var priceList = PriceList.Create(
            request.Name,
            request.Description,
            request.PriceListName,
            request.PriceListType,
            request.EffectiveDate,
            request.ExpiryDate,
            request.Currency,
            request.MinimumOrderValue,
            request.CustomerType,
            request.Notes);

        await repository.AddAsync(priceList, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("PriceList created {PriceListId}", priceList.Id);
        return new CreatePriceListResponse(priceList.Id);
    }
}

