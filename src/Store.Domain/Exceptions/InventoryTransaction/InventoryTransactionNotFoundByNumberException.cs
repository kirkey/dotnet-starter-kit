using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InventoryTransactionNotFoundByNumberException(string transactionNumber)
    : NotFoundException($"Inventory Transaction with Number '{transactionNumber}' was not found.") {}

