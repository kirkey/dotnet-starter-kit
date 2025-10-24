using Store.Domain.Exceptions.GoodsReceipt;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

public sealed class CreateGoodsReceiptHandler(
    [FromKeyedServices("store:goodsreceipts")] IRepository<GoodsReceipt> repository)
    : IRequestHandler<CreateGoodsReceiptCommand, CreateGoodsReceiptResponse>
{
    public async Task<CreateGoodsReceiptResponse> Handle(CreateGoodsReceiptCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate receipt number
        var existingReceipt = await repository.FirstOrDefaultAsync(
            new GoodsReceiptByNumberSpec(request.ReceiptNumber), cancellationToken).ConfigureAwait(false);

        if (existingReceipt is not null)
        {
            throw new GoodsReceiptAlreadyExistsException(request.ReceiptNumber);
        }

        var goodsReceipt = GoodsReceipt.Create(
            request.ReceiptNumber,
            request.ReceivedDate,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.PurchaseOrderId,
            request.Notes);
        if (!string.IsNullOrWhiteSpace(request.Name)) goodsReceipt.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Description)) goodsReceipt.Description = request.Description;
        await repository.AddAsync(goodsReceipt, cancellationToken).ConfigureAwait(false);

        return new CreateGoodsReceiptResponse(goodsReceipt.Id);
    }
}
