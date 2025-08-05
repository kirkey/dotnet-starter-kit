using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

// InventoryItem Exceptions
public sealed class InventoryItemNotFoundException(DefaultIdType id) : NotFoundException($"inventory item with id {id} not found");
public sealed class InsufficientStockException(DefaultIdType itemId, decimal requested, decimal available) 
    : ForbiddenException($"insufficient stock for item {itemId}. Requested: {requested}, Available: {available}");
public sealed class InvalidInventoryQuantityException() : ForbiddenException("inventory quantity must be non-negative");
public sealed class InvalidInventoryUnitPriceException() : ForbiddenException("inventory unit price cannot be negative");
public sealed class InvalidInventoryReorderLevelException() : ForbiddenException("reorder level cannot be negative");
public sealed class InventoryItemAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"inventory item with id {id} is already inactive");

// CostingMethod Exceptions
public sealed class CostingMethodNotFoundException(DefaultIdType id) : NotFoundException($"costing method with id {id} not found");
public sealed class CostingMethodAlreadyActiveException(DefaultIdType id) : ForbiddenException($"costing method with id {id} is already active");
public sealed class CostingMethodAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"costing method with id {id} is already inactive");

// Project Exceptions
public sealed class ProjectNotFoundException(DefaultIdType id) : NotFoundException($"project with id {id} not found");
public sealed class ProjectAlreadyCompletedException(DefaultIdType id) : ForbiddenException($"project with id {id} is already completed");
public sealed class ProjectAlreadyCancelledException(DefaultIdType id) : ForbiddenException($"project with id {id} is already cancelled");
public sealed class ProjectCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"project with id {id} cannot be modified after completion or cancellation");
public sealed class InvalidProjectBudgetException() : ForbiddenException("project budget amount cannot be negative");
public sealed class InvalidProjectCostEntryException() : ForbiddenException("project cost entry amount must be positive");
public sealed class InvalidProjectRevenueEntryException() : ForbiddenException("project revenue entry amount must be positive");
public sealed class JobCostingEntryNotFoundException(DefaultIdType id) : NotFoundException($"job costing entry with id {id} not found");
