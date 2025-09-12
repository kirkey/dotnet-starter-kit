using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionNotApprovedException(DefaultIdType id)
    : ForbiddenException($"Inventory Transaction with ID '{id}' is not approved.") {}
