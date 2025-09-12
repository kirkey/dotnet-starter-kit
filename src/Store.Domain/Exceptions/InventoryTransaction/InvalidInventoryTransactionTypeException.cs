using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransaction;

public sealed class InvalidInventoryTransactionTypeException(string transactionType)
    : ForbiddenException($"Invalid inventory transaction type: '{transactionType}'. Valid types are: IN, OUT, ADJUSTMENT, TRANSFER.") {}

