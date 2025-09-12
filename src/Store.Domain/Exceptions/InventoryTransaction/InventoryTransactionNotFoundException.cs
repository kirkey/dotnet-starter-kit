using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionNotFoundException(DefaultIdType id)
    : NotFoundException($"Inventory Transaction with ID '{id}' was not found.") {}
