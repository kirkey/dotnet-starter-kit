namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionAlreadyApprovedException(DefaultIdType id)
    : CustomException($"Inventory Transaction with ID '{id}' is already approved.");
