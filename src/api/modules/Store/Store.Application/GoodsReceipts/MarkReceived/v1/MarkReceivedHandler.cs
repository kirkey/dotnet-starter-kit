using Store.Domain.Exceptions.GoodsReceipt;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.MarkReceived.v1;

public sealed class MarkReceivedHandler(
    [FromKeyedServices("store:goodsreceipts")] IRepository<GoodsReceipt> repository)
    : IRequestHandler<MarkReceivedCommand, MarkReceivedResponse>
{
    public async Task<MarkReceivedResponse> Handle(MarkReceivedCommand request, CancellationToken cancellationToken)
    {
        var goodsReceipt = await repository.GetByIdAsync(request.GoodsReceiptId, cancellationToken).ConfigureAwait(false)
            ?? throw new GoodsReceiptNotFoundException(request.GoodsReceiptId);

        goodsReceipt.MarkReceived();

        await repository.UpdateAsync(goodsReceipt, cancellationToken).ConfigureAwait(false);

        return new MarkReceivedResponse
        {
            Success = true
        };
    }
}
