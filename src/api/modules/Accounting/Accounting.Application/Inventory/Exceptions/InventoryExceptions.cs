namespace Accounting.Application.Inventory.Exceptions;

public class InventoryNotFoundException(DefaultIdType id) : NotFoundException($"Inventory item with id {id} was not found.");

public class InventorySkuAlreadyExistsException(string sku) : ForbiddenException($"Inventory item with SKU '{sku}' already exists.");

public class InsufficientStockAppException(DefaultIdType itemId, decimal requested, decimal available)
    : ForbiddenException($"Insufficient stock for item {itemId}. Requested: {requested}, Available: {available}");

