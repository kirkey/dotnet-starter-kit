using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Get.v1;

public sealed class GetItemHandler(
    [FromKeyedServices("store:items")] IReadRepository<Item> repository)
    : IRequestHandler<GetItemCommand, ItemResponse>
{
    public async Task<ItemResponse> Handle(GetItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await repository.FirstOrDefaultAsync(
            new Specs.GetItemByIdSpec(request.Id), 
            cancellationToken).ConfigureAwait(false);

        if (item is null)
            throw new ItemNotFoundException(request.Id);

        return new ItemResponse(
            item.Id,
            item.Name,
            item.Description,
            item.Sku,
            item.Barcode,
            item.UnitPrice,
            item.Cost,
            item.MinimumStock,
            item.MaximumStock,
            item.ReorderPoint,
            item.ReorderQuantity,
            item.LeadTimeDays,
            item.IsPerishable,
            item.IsSerialTracked,
            item.IsLotTracked,
            item.ShelfLifeDays,
            item.Brand,
            item.Manufacturer,
            item.ManufacturerPartNumber,
            item.Weight,
            item.WeightUnit,
            item.Length,
            item.Width,
            item.Height,
            item.DimensionUnit,
            item.CategoryId,
            item.SupplierId,
            item.UnitOfMeasure,
            item.CreatedOn,
            item.CreatedBy);
    }
}
