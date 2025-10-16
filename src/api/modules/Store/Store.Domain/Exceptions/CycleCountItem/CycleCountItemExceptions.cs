namespace Store.Domain.Exceptions.CycleCountItem;

/// <summary>
/// Exception thrown when a cycle count item is not found by ID.
/// </summary>
public sealed class CycleCountItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"cycle count item with id {id} not found");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a cycle count.
/// </summary>
public sealed class DuplicateCycleCountItemException(DefaultIdType cycleCountId, DefaultIdType itemId) : ConflictException($"item {itemId} already exists in cycle count {cycleCountId}");

/// <summary>
/// Exception thrown when counted quantity is invalid.
/// </summary>
public sealed class InvalidCountedQuantityException() : ForbiddenException("counted quantity cannot be negative");

/// <summary>
/// Exception thrown when trying to modify a cycle count item after the count is finalized.
/// </summary>
public sealed class CannotModifyFinalizedCycleCountItemException(DefaultIdType id) : ForbiddenException($"cannot modify cycle count item with id {id} after count is finalized");

/// <summary>
/// Exception thrown when variance threshold is exceeded.
/// </summary>
public sealed class VarianceThresholdExceededException(decimal variance, decimal threshold) : ForbiddenException($"variance {variance} exceeds allowed threshold {threshold}");

/// <summary>
/// Exception thrown when trying to count an item that doesn't exist in the cycle count.
/// </summary>
public sealed class ItemNotInCycleCountException(DefaultIdType itemId, DefaultIdType cycleCountId) : ForbiddenException($"item {itemId} is not included in cycle count {cycleCountId}");

/// <summary>
/// Exception thrown when a cycle count item is not found.
/// </summary>
public sealed class CycleCountItemNotFoundException(DefaultIdType id) : NotFoundException($"cycle count item with id {id} not found");

