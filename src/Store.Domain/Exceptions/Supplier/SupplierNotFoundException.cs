namespace Store.Domain.Exceptions.Supplier;

/// <summary>
/// Exception thrown when a supplier with the provided identifier cannot be found.
/// </summary>
public sealed class SupplierNotFoundException(DefaultIdType id)
    : NotFoundException($"Supplier with ID '{id}' was not found.") {}
