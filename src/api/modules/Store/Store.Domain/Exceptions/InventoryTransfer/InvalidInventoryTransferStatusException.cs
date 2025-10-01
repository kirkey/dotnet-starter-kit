namespace Store.Domain.Exceptions.InventoryTransfer;

/// <summary>
/// Exception thrown when an attempted operation is not valid for the current inventory transfer status.
/// </summary>
public sealed class InvalidInventoryTransferStatusException(string attemptedOperation, string currentStatus)
    : ConflictException($"Operation '{attemptedOperation}' is not valid when transfer status is '{currentStatus}'.") {}

