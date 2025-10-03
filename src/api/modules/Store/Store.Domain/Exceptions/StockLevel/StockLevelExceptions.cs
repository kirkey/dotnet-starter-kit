namespace Store.Domain.Exceptions.StockLevel;

/// <summary>
/// Exception thrown when a stock level record is not found.
/// </summary>
public class StockLevelNotFoundException : NotFoundException
{
    public StockLevelNotFoundException(DefaultIdType stockLevelId)
        : base($"Stock level with ID '{stockLevelId}' was not found.")
    {
    }

    public StockLevelNotFoundException(DefaultIdType itemId, DefaultIdType warehouseId)
        : base($"Stock level for item '{itemId}' in warehouse '{warehouseId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when there is insufficient stock for an operation.
/// </summary>
public class InsufficientStockException(DefaultIdType itemId, DefaultIdType warehouseId, int available, int required)
    : BadRequestException($"Insufficient stock for item '{itemId}' in warehouse '{warehouseId}'. Available: {available}, Required: {required}");

/// <summary>
/// Exception thrown when stock level operation is invalid.
/// </summary>
public class InvalidStockLevelOperationException(string message) : BadRequestException(message);
