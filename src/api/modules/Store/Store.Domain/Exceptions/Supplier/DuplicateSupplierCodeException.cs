namespace Store.Domain.Exceptions.Supplier;

/// <summary>
/// Exception thrown when attempting to create a supplier with a code that already exists.
/// </summary>
public sealed class DuplicateSupplierCodeException(string code)
    : ConflictException($"Supplier with Code '{code}' already exists.") {}

