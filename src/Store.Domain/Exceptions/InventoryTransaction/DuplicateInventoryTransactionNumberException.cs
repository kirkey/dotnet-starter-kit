namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class DuplicateInventoryTransactionNumberException(string transactionNumber)
    : ConflictException($"Inventory Transaction with Number '{transactionNumber}' already exists.") {}

