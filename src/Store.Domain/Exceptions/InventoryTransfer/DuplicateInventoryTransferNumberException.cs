using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.InventoryTransfer;

public sealed class DuplicateInventoryTransferNumberException(string transferNumber)
    : BadRequestException($"An inventory transfer with number '{transferNumber}' already exists.") {}

