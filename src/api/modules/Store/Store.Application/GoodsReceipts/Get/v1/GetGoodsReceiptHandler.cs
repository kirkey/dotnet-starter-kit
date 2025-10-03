using Store.Domain.Exceptions.GoodsReceipt;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;

public sealed class GetGoodsReceiptHandler(
    [FromKeyedServices("store:goodsreceipts")] IReadRepository<GoodsReceipt> repository)
    : IRequestHandler<GetGoodsReceiptCommand, GetGoodsReceiptResponse>
{
    public async Task<GetGoodsReceiptResponse> Handle(GetGoodsReceiptCommand request, CancellationToken cancellationToken)
    {
        var goodsReceipt = await repository.FirstOrDefaultAsync(
            new GetGoodsReceiptByIdSpec(request.GoodsReceiptId), cancellationToken).ConfigureAwait(false)
            ?? throw new GoodsReceiptNotFoundException(request.GoodsReceiptId);

        return new GetGoodsReceiptResponse
        {
            Id = goodsReceipt.Id,
            ReceiptNumber = goodsReceipt.ReceiptNumber,
            PurchaseOrderId = goodsReceipt.PurchaseOrderId,
            ReceivedDate = goodsReceipt.ReceivedDate,
            Status = goodsReceipt.Status,
            Items = goodsReceipt.Items.Select(item => new GoodsReceiptItemDto
            {
                Id = item.Id,
                ItemId = item.ItemId,
                Name = item.Name,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}
