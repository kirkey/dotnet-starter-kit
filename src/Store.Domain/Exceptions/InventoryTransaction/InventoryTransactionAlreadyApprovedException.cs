namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionAlreadyApprovedException(DefaultIdType id)
    : Exception($"Inventory Transaction with ID '{id}' is already approved.");
