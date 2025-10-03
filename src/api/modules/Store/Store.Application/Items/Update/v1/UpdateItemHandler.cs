using Store.Domain.Exceptions.Items;

namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

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

        item.Update(
            request.Name,
            request.Description,
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

        await repository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("item updated {ItemId}", item.Id);
        return new UpdateItemResponse(item.Id);
    }
}
