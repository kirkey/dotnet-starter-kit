using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.Warehouse;

public sealed class WarehouseNotFoundByCodeException(string code)
    : NotFoundException($"Warehouse with Code '{code}' was not found.") {}
