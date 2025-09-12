using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransfer;

public sealed class InventoryTransferNotFoundException(DefaultIdType id)
    : NotFoundException($"Inventory Transfer with ID '{id}' was not found.") {}
