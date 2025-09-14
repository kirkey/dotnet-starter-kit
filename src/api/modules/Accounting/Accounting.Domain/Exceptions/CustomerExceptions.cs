namespace Accounting.Domain.Exceptions;

// Customer Exceptions
public sealed class CustomerByIdNotFoundException(DefaultIdType id) : NotFoundException($"customer with id {id} not found");
public sealed class CustomerByCodeNotFoundException(string customerCode) : NotFoundException($"customer with code {customerCode} not found");
public sealed class CustomerByNameNotFoundException(string name) : NotFoundException($"customer with name {name} not found");
public sealed class CustomerAlreadyActiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already active");
public sealed class CustomerAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already inactive");
public sealed class InvalidCustomerCreditLimitException() : ForbiddenException("customer credit limit cannot be negative");
public sealed class CustomerCreditLimitExceededException(DefaultIdType customerId, decimal currentBalance, decimal creditLimit)
    : ForbiddenException($"customer {customerId} credit limit exceeded. Current balance: {currentBalance}, Credit limit: {creditLimit}");
public sealed class InvalidCustomerBalanceTransactionException() : ForbiddenException("customer balance transaction amount must be positive");

