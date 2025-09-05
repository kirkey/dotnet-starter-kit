using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Exceptions;

public class WarehouseNotFoundException : NotFoundException
{
    public WarehouseNotFoundException(DefaultIdType id) : base($"Warehouse with ID {id} was not found.")
    {
    }

    public WarehouseNotFoundException(string code) : base($"Warehouse with code '{code}' was not found.")
    {
    }
}

public class WarehouseAlreadyExistsException(string code)
    : ConflictException($"Warehouse with code '{code}' already exists.");

public class InactiveWarehouseException(DefaultIdType warehouseId)
    : BadRequestException($"Warehouse {warehouseId} is inactive and cannot be used for operations.");

public class StockMovementNotFoundException(DefaultIdType id) : NotFoundException($"Stock movement with ID {id} was not found.");

public class InvalidStockMovementException(string message) : BadRequestException(message);

public class InsufficientStockException(string productSku, decimal available, decimal required)
    : BadRequestException($"Insufficient stock for product {productSku}. Available: {available}, Required: {required}");

public class InventoryItemNotFoundException(DefaultIdType warehouseId, string productSku)
    : NotFoundException($"Inventory item for product {productSku} in warehouse {warehouseId} was not found.");

public class CapacityExceededException(string message) : BadRequestException(message);
