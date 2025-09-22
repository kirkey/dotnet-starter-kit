namespace Store.Domain.Exceptions.Warehouse;

public sealed class WarehouseInactiveException(DefaultIdType id)
    : CustomException($"Warehouse with ID '{id}' is inactive.") {}
