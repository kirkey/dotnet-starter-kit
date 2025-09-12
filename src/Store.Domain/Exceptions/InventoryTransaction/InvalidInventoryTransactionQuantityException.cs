using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InvalidInventoryTransactionQuantityException(int quantity)
    : ForbiddenException($"Inventory transaction quantity must be greater than zero. Provided: {quantity}.") {}
