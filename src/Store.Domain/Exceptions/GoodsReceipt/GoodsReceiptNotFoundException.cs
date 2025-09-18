namespace Store.Domain.Exceptions.GoodsReceipt;

public sealed class GoodsReceiptNotFoundException(DefaultIdType id)
    : NotFoundException($"Goods Receipt with ID '{id}' was not found.") {}

