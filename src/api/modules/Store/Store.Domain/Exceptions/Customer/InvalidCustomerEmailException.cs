namespace Store.Domain.Exceptions.Customer;

/// <summary>
/// Thrown when a customer's email does not match the required format.
/// </summary>
public sealed class InvalidCustomerEmailException(string email)
    : CustomException($"Customer email '{email}' is invalid.") {}

