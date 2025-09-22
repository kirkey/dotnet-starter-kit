namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when attempting to delete a warehouse that has transaction history.
/// </summary>
/// <param name="warehouseId">The ID of the warehouse</param>
/// <param name="transactionCount">Number of transactions associated with the warehouse</param>
public sealed class WarehouseDeletionNotAllowedException(DefaultIdType warehouseId, int transactionCount)
    : CustomException($"Cannot delete warehouse '{warehouseId}' because it has {transactionCount} transaction records. Warehouses with historical data cannot be deleted.") { }
