namespace Store.Domain.Exceptions.StockAdjustment;

public sealed class StockAdjustmentNotFoundException(DefaultIdType id)
    : NotFoundException($"Stock Adjustment with ID '{id}' was not found.") {}

public sealed class StockAdjustmentNotFoundByNumberException(string adjustmentNumber)
    : NotFoundException($"Stock Adjustment with Number '{adjustmentNumber}' was not found.") {}

