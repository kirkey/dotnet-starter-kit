namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when attempting to deactivate a warehouse that contains current inventory.
/// </summary>
/// <param name="warehouseId">The ID of the warehouse</param>
/// <param name="currentInventoryCount">Number of inventory items in the warehouse</param>
public sealed class WarehouseDeactivationNotAllowedException(DefaultIdType warehouseId, int currentInventoryCount)
    : CustomException($"Cannot deactivate warehouse '{warehouseId}' because it contains {currentInventoryCount} inventory items. Move or remove all inventory before deactivating.") { }
