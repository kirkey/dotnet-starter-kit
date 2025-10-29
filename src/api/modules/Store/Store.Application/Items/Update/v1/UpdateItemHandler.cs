using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Handler for UpdateItemCommand.
/// Updates Item entity by calling all three update methods: Update, UpdateStockLevels, and UpdatePhysicalAttributes.
/// Handles image upload if provided.
/// </summary>
public sealed class UpdateItemHandler(
    ILogger<UpdateItemHandler> logger,
    [FromKeyedServices("store:items")] IRepository<Item> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateItemCommand, UpdateItemResponse>
{
    public async Task<UpdateItemResponse> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            throw new ItemNotFoundException(request.Id);

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

        // Update basic details
        item.Update(
            request.Name,
            request.Description,
            request.Notes,
            request.Sku,
            request.Barcode,
            request.UnitPrice,
            request.Cost,
            request.CategoryId,
            request.SupplierId,
            request.Brand,
            request.Manufacturer,
            request.ManufacturerPartNumber,
            request.UnitOfMeasure);

        // Update stock control parameters
        item.UpdateStockLevels(
            request.MinimumStock,
            request.MaximumStock,
            request.ReorderPoint,
            request.ReorderQuantity,
            request.LeadTimeDays);

        // Update physical attributes
        item.UpdatePhysicalAttributes(
            request.Weight,
            request.WeightUnit,
            request.Length,
            request.Width,
            request.Height,
            request.DimensionUnit);

        // Update tracking settings
        item.UpdateTrackingSettings(
            request.IsPerishable,
            request.IsSerialTracked,
            request.IsLotTracked,
            request.ShelfLifeDays);

        // Update image URL if changed
        if (!string.IsNullOrWhiteSpace(imageUrl) && item.ImageUrl != imageUrl)
        {
            item.ImageUrl = imageUrl;
        }

        await repository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Item updated {ItemId}. ImageUrl: {ImageUrl}", item.Id, imageUrl);
        return new UpdateItemResponse(item.Id);
    }
}
