namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when a warehouse with the specified code cannot be found.
/// </summary>
/// <param name="code">The code of the warehouse that was not found.</param>
public sealed class WarehouseNotFoundByCodeException(string code)
    : NotFoundException($"Warehouse with Code '{code}' was not found.") {}
