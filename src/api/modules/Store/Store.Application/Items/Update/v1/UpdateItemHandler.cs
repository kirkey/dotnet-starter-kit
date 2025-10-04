using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Handler for UpdateItemCommand.
/// Updates Item entity by calling all three update methods: Update, UpdateStockLevels, and UpdatePhysicalAttributes.
/// </summary>
public sealed class UpdateItemHandler(
    ILogger<UpdateItemHandler> logger,
    [FromKeyedServices("store:items")] IRepository<Item> repository)
    : IRequestHandler<UpdateItemCommand, UpdateItemResponse>
{
    public async Task<UpdateItemResponse> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (item is null)
            throw new ItemNotFoundException(request.Id);

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

        await repository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("item updated {ItemId}", item.Id);
        return new UpdateItemResponse(item.Id);
    }
}
