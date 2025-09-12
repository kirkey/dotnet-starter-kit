namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionAlreadyApprovedException : Exception
{
    public InventoryTransactionAlreadyApprovedException(DefaultIdType id)
        : base($"Inventory Transaction with ID '{id}' is already approved.") {}
}
