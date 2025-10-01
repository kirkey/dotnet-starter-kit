namespace Store.Domain.Exceptions.Warehouse;

/// <summary>
/// Exception thrown when attempting to duplicate a warehouse code that already exists in the system.
/// </summary>
/// <param name="code">The duplicate warehouse code</param>
public sealed class DuplicateWarehouseCodeException(string code)
    : CustomException($"Warehouse with code '{code}' already exists. Warehouse codes must be unique.") { }
