using Store.Domain.Exceptions.PriceList;

namespace FSH.Starter.WebApi.Store.Application.PriceLists.Get.v1;

public sealed class GetPriceListHandler(
    ILogger<GetPriceListHandler> logger,
    [FromKeyedServices("store:price-lists")] IRepository<PriceList> repository)
    : IRequestHandler<GetPriceListQuery, GetPriceListResponse>
{
    public async Task<GetPriceListResponse> Handle(GetPriceListQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetPriceListSpecification(request.Id);
        var priceList = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = priceList ?? throw new PriceListNotFoundException(request.Id);

        return new GetPriceListResponse(
            priceList.Id,
            priceList.Name,
            priceList.Description,
            priceList.PriceListName,
            priceList.PriceListType,
            priceList.EffectiveDate,
            priceList.ExpiryDate,
            priceList.IsActive,
            priceList.Currency,
            priceList.MinimumOrderValue,
            priceList.CustomerType,
            priceList.Notes,
            priceList.CreatedOn,
            priceList.LastModifiedOn);
    }
}
