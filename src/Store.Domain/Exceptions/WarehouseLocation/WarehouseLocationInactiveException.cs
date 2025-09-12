namespace Store.Domain.Exceptions.WarehouseLocation;

public sealed class WarehouseLocationInactiveException(DefaultIdType id)
    : Exception($"Warehouse Location with ID '{id}' is inactive.") {}
