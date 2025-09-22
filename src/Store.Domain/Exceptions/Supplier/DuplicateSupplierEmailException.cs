namespace Store.Domain.Exceptions.Supplier;

/// <summary>
/// Exception thrown when attempting to create a supplier with an email that already exists.
/// </summary>
public sealed class DuplicateSupplierEmailException(string email)
    : ConflictException($"Supplier with Email '{email}' already exists.") {}

