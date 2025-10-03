using Store.Domain.Exceptions.GoodsReceipt;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;

public sealed class AddGoodsReceiptItemHandler(
    [FromKeyedServices("store:goodsreceipts")] IRepository<GoodsReceipt> repository)
    : IRequestHandler<AddGoodsReceiptItemCommand, AddGoodsReceiptItemResponse>
{
    public async Task<AddGoodsReceiptItemResponse> Handle(AddGoodsReceiptItemCommand request, CancellationToken cancellationToken)
    {
        var goodsReceipt = await repository.GetByIdAsync(request.GoodsReceiptId, cancellationToken).ConfigureAwait(false)
            ?? throw new GoodsReceiptNotFoundException(request.GoodsReceiptId);

        goodsReceipt.AddItem(request.ItemId, request.Name, request.Quantity);

        await repository.UpdateAsync(goodsReceipt, cancellationToken).ConfigureAwait(false);

        return new AddGoodsReceiptItemResponse
        {
            Success = true
        };
    }
}
