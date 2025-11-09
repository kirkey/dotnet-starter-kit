using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Store.Application.Items.Specs;
using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Create.v1;

public sealed class CreateItemHandler(
    ILogger<CreateItemHandler> logger,
    [FromKeyedServices("store:items")] IRepository<Item> repository,
    [FromKeyedServices("store:items")] IReadRepository<Item> readRepository,
    IStorageService storageService)
    : IRequestHandler<CreateItemCommand, CreateItemResponse>
{
    /// <summary>
    /// Creates a new item. If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    public async Task<CreateItemResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate SKU
        var existingBySku = await readRepository.FirstOrDefaultAsync(
            new ItemBySkuSpec(request.Sku!), 
            cancellationToken).ConfigureAwait(false);
        
        if (existingBySku is not null)
            throw new DuplicateItemSkuException(request.Sku!);

        // Check for duplicate Barcode
        var existingByBarcode = await readRepository.FirstOrDefaultAsync(
            new ItemByBarcodeSpec(request.Barcode!), 
            cancellationToken).ConfigureAwait(false);
        
        if (existingByBarcode is not null)
            throw new DuplicateItemBarcodeException(request.Barcode!);

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Item>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

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

        // Set the image URL after creation
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            item.ImageUrl = imageUrl;
        }

        await repository.AddAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Item created {ItemId}. ImageUrl: {ImageUrl}", item.Id, imageUrl);
        return new CreateItemResponse(item.Id);
    }
}
