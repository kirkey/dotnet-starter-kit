namespace Store.Domain.Exceptions.Customer;

/// <summary>
/// Thrown when a requested operation would exceed the customer's credit limit.
/// </summary>
public sealed class CustomerCreditLimitExceededException(DefaultIdType id, decimal creditLimit, decimal attempted)
    : CustomException($"Customer '{id}' credit limit {creditLimit:N2} exceeded by attempted amount {attempted:N2}.") {}


