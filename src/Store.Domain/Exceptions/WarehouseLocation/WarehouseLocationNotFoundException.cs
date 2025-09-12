using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.WarehouseLocation;

public sealed class WarehouseLocationNotFoundException(DefaultIdType id)
    : NotFoundException($"Warehouse Location with ID '{id}' was not found.") {}
