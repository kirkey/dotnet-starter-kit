using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Create.v1;

public sealed class CreateItemHandler(
    ILogger<CreateItemHandler> logger,
    [FromKeyedServices("store:items")] IRepository<Item> repository,
    [FromKeyedServices("store:items")] IReadRepository<Item> readRepository)
    : IRequestHandler<CreateItemCommand, CreateItemResponse>
{
    public async Task<CreateItemResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate SKU
        var existingBySku = await readRepository.FirstOrDefaultAsync(
            new Specs.ItemBySkuSpec(request.Sku!), 
            cancellationToken).ConfigureAwait(false);
        
        if (existingBySku is not null)
            throw new DuplicateItemSkuException(request.Sku!);

        // Check for duplicate Barcode
        var existingByBarcode = await readRepository.FirstOrDefaultAsync(
            new Specs.ItemByBarcodeSpec(request.Barcode!), 
            cancellationToken).ConfigureAwait(false);
        
        if (existingByBarcode is not null)
            throw new DuplicateItemBarcodeException(request.Barcode!);

        var item = Item.Create(
            request.Name!,
            request.Description,
            request.Sku!,
            request.Barcode!,
            request.UnitPrice,
            request.Cost,
            request.MinimumStock,
            request.MaximumStock,
            request.ReorderPoint,
            request.ReorderQuantity,
            request.LeadTimeDays,
            request.CategoryId!.Value,
            request.SupplierId!.Value,
            request.UnitOfMeasure,
            request.IsPerishable,
            request.IsSerialTracked,
            request.IsLotTracked,
            request.ShelfLifeDays,
            request.Brand,
            request.Manufacturer,
            request.ManufacturerPartNumber,
            request.Weight,
            request.WeightUnit,
            request.Length,
            request.Width,
            request.Height,
            request.DimensionUnit);

        await repository.AddAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("item created {ItemId}", item.Id);
        return new CreateItemResponse(item.Id);
    }
}
