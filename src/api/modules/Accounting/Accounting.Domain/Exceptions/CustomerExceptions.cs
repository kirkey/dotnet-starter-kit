// Customer Exceptions

// Customer Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a customer is not found by ID.
/// </summary>
public sealed class CustomerByIdNotFoundException(DefaultIdType id) : NotFoundException($"customer with id {id} not found");

/// <summary>
/// Exception thrown when a customer is not found by customer number.
/// </summary>
public sealed class CustomerByNumberNotFoundException(string customerNumber) : NotFoundException($"customer with number {customerNumber} not found");

/// <summary>
/// Exception thrown when trying to create a customer with a duplicate number.
/// </summary>
public sealed class DuplicateCustomerNumberException(string customerNumber) : ConflictException($"customer with number {customerNumber} already exists");

/// <summary>
/// Exception thrown when credit limit is invalid (negative).
/// </summary>
public sealed class InvalidCreditLimitException() : ForbiddenException("credit limit cannot be negative");

/// <summary>
/// Exception thrown when discount percentage is out of range.
/// </summary>
public sealed class InvalidDiscountPercentageException() : ForbiddenException("discount percentage must be between 0 and 1 (0% to 100%)");

/// <summary>
/// Exception thrown when trying to place an already held customer on credit hold.
/// </summary>
public sealed class CustomerAlreadyOnCreditHoldException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already on credit hold");

/// <summary>
/// Exception thrown when trying to remove credit hold from a customer not on hold.
/// </summary>
public sealed class CustomerNotOnCreditHoldException(DefaultIdType id) : ForbiddenException($"customer with id {id} is not on credit hold");

/// <summary>
/// Exception thrown when a customer exceeds their credit limit.
/// </summary>
public sealed class CustomerOverCreditLimitException(string customerNumber, decimal currentBalance, decimal creditLimit) 
    : ForbiddenException($"customer {customerNumber} balance {currentBalance:C} exceeds credit limit {creditLimit:C}");

/// <summary>
/// Exception thrown when trying to deactivate an already inactive customer.
/// </summary>
public sealed class CustomerAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already inactive");

/// <summary>
/// Exception thrown when trying to activate an already active customer.
/// </summary>
public sealed class CustomerAlreadyActiveException(DefaultIdType id) : ForbiddenException($"customer with id {id} is already active");

/// <summary>
/// Exception thrown when required billing address is missing.
/// </summary>
public sealed class BillingAddressRequiredException() : ForbiddenException("billing address is required for customer creation");

/// <summary>
/// Exception thrown when trying to transact with an inactive customer.
/// </summary>
public sealed class CannotTransactWithInactiveCustomerException(DefaultIdType id) : ForbiddenException($"cannot create transactions for inactive customer {id}");

/// <summary>
/// Exception thrown when trying to invoice a customer on credit hold.
/// </summary>
public sealed class CannotInvoiceCustomerOnCreditHoldException(DefaultIdType id) : ForbiddenException($"cannot create invoices for customer {id} on credit hold");
