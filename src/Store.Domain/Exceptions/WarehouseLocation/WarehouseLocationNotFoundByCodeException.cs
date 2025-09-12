using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.WarehouseLocation;

public sealed class WarehouseLocationNotFoundByCodeException(string code)
    : NotFoundException($"Warehouse Location with Code '{code}' was not found.") {}
