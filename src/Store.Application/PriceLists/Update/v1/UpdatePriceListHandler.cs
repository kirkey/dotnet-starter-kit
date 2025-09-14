using Store.Domain.Exceptions.PriceList;

namespace FSH.Starter.WebApi.Store.Application.PriceLists.Update.v1;

public sealed class UpdatePriceListHandler(
    ILogger<UpdatePriceListHandler> logger,
    [FromKeyedServices("store:price-lists")] IRepository<PriceList> repository)
    : IRequestHandler<UpdatePriceListCommand, UpdatePriceListResponse>
{
    public async Task<UpdatePriceListResponse> Handle(UpdatePriceListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var priceList = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = priceList ?? throw new PriceListNotFoundException(request.Id);

        var updated = priceList.Update(
            request.Name,
            request.Description,
            request.PriceListName,
            request.PriceListType,
            request.EffectiveDate,
            request.ExpiryDate,
            request.IsActive,
            request.Currency,
            request.MinimumOrderValue,
            request.CustomerType,
            request.Notes);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("price list with id : {PriceListId} updated.", priceList.Id);
        return new UpdatePriceListResponse(priceList.Id);
    }
}

