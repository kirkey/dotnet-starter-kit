using FSH.Starter.WebApi.Store.Application.Items.Specs;

namespace FSH.Starter.WebApi.Store.Application.Items.Search.v1;

public sealed class SearchItemsHandler(
    [FromKeyedServices("store:items")] IReadRepository<Item> repository)
    : IRequestHandler<SearchItemsCommand, PagedList<ItemResponse>>
{
    public async Task<PagedList<ItemResponse>> Handle(SearchItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchItemsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var itemResponses = items.Select(i => new ItemResponse(
            i.Id,
            i.Name,
            i.Description,
            i.Sku,
            i.Barcode,
            i.UnitPrice,
            i.Cost,
            i.MinimumStock,
            i.MaximumStock,
            i.ReorderPoint,
            i.ReorderQuantity,
            i.LeadTimeDays,
            i.IsPerishable,
            i.IsSerialTracked,
            i.IsLotTracked,
            i.ShelfLifeDays,
            i.Brand,
            i.Manufacturer,
            i.ManufacturerPartNumber,
            i.Weight,
            i.WeightUnit,
            i.Length,
            i.Width,
            i.Height,
            i.DimensionUnit,
            i.CategoryId,
            i.SupplierId,
            i.UnitOfMeasure,
            i.CreatedOn,
            i.CreatedBy)).ToList();

        return new PagedList<ItemResponse>(itemResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
