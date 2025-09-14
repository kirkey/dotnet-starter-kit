using Store.Domain.Exceptions.PriceList;

namespace FSH.Starter.WebApi.Store.Application.PriceLists.Delete.v1;

public sealed class DeletePriceListHandler(
    ILogger<DeletePriceListHandler> logger,
    [FromKeyedServices("store:price-lists")] IRepository<PriceList> repository)
    : IRequestHandler<DeletePriceListCommand>
{
    public async Task Handle(DeletePriceListCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var priceList = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = priceList ?? throw new PriceListNotFoundException(request.Id);

        await repository.DeleteAsync(priceList, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("price list deleted {PriceListId}", priceList.Id);
    }
}

