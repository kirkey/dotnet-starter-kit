// Payment Allocation Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a payment allocation is not found by ID.
/// </summary>
public sealed class PaymentAllocationByIdNotFoundException(DefaultIdType id) : NotFoundException($"payment allocation with id {id} not found");

/// <summary>
/// Exception thrown when trying to allocate more than the available payment amount.
/// </summary>
public sealed class InsufficientPaymentAmountException(decimal availableAmount, decimal requestedAmount) 
    : ForbiddenException($"cannot allocate {requestedAmount:C}. Only {availableAmount:C} available");

/// <summary>
/// Exception thrown when a payment allocation amount is invalid (negative or zero).
/// </summary>
public sealed class InvalidPaymentAllocationAmountException() : ForbiddenException("payment allocation amount must be positive");

/// <summary>
/// Exception thrown when trying to allocate to an invoice that is already fully paid.
/// </summary>
public sealed class CannotAllocateToFullyPaidInvoiceException(DefaultIdType invoiceId) : ForbiddenException($"cannot allocate payment to fully paid invoice with id {invoiceId}");

/// <summary>
/// Exception thrown when trying to modify an allocation after the payment is processed.
/// </summary>
public sealed class CannotModifyAllocationOfProcessedPaymentException(DefaultIdType paymentId) : ForbiddenException($"cannot modify allocation of processed payment with id {paymentId}");

/// <summary>
/// Exception thrown when trying to allocate more than the invoice outstanding balance.
/// </summary>
public sealed class AllocationExceedsInvoiceBalanceException(decimal invoiceBalance, decimal allocationAmount) 
    : ForbiddenException($"allocation amount {allocationAmount:C} exceeds invoice outstanding balance {invoiceBalance:C}");

/// <summary>
/// Exception thrown when the payment and invoice belong to different customers.
/// </summary>
public sealed class PaymentInvoiceCustomerMismatchException(DefaultIdType paymentId, DefaultIdType invoiceId) 
    : ForbiddenException($"payment {paymentId} and invoice {invoiceId} belong to different customers");
