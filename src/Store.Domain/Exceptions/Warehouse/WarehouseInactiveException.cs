namespace Store.Domain.Exceptions.Warehouse;

public sealed class WarehouseInactiveException(DefaultIdType id)
    : Exception($"Warehouse with ID '{id}' is inactive.") {}
