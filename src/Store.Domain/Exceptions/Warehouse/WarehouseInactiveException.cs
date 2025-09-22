namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when an operation is attempted against an inactive warehouse.
/// </summary>
/// <param name="id">The ID of the inactive warehouse.</param>
public sealed class WarehouseInactiveException(DefaultIdType id)
    : CustomException($"Warehouse with ID '{id}' is inactive.") {}
