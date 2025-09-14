using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransfer;

public sealed class InventoryTransferItemNotFoundException(DefaultIdType id)
    : NotFoundException($"Inventory transfer item with ID '{id}' was not found.") {}

