namespace Store.Domain.Exceptions.Customer;

/// <summary>
/// Thrown when a customer with the given email already exists.
/// </summary>
public sealed class DuplicateCustomerEmailException(string email)
    : ConflictException($"Customer with email '{email}' already exists.") {}
