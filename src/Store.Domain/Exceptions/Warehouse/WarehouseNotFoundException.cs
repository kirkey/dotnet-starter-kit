using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.Warehouse;

public sealed class WarehouseNotFoundException(DefaultIdType id)
    : NotFoundException($"Warehouse with ID '{id}' was not found.") {}
