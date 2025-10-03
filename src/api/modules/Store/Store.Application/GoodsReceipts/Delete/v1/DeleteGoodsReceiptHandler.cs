using Store.Domain.Exceptions.GoodsReceipt;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Delete.v1;

public sealed class DeleteGoodsReceiptHandler(
    [FromKeyedServices("store:goodsreceipts")] IRepository<GoodsReceipt> repository)
    : IRequestHandler<DeleteGoodsReceiptCommand, DeleteGoodsReceiptResponse>
{
    public async Task<DeleteGoodsReceiptResponse> Handle(DeleteGoodsReceiptCommand request, CancellationToken cancellationToken)
    {
        var goodsReceipt = await repository.GetByIdAsync(request.GoodsReceiptId, cancellationToken).ConfigureAwait(false)
            ?? throw new GoodsReceiptNotFoundException(request.GoodsReceiptId);

        await repository.DeleteAsync(goodsReceipt, cancellationToken).ConfigureAwait(false);

        return new DeleteGoodsReceiptResponse
        {
            Success = true
        };
    }
}
