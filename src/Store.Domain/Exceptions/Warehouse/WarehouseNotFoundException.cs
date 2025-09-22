namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when a warehouse with the specified ID cannot be found.
/// </summary>
/// <param name="id">The ID of the warehouse that was not found.</param>
public sealed class WarehouseNotFoundException(DefaultIdType id)
    : NotFoundException($"Warehouse with ID '{id}' was not found.") {}
