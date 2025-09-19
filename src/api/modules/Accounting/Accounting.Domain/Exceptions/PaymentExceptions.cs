// Payment Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a payment is not found by ID.
/// </summary>
public sealed class PaymentByIdNotFoundException(DefaultIdType id) : NotFoundException($"payment with id {id} not found");

/// <summary>
/// Exception thrown when a payment is not found by payment number.
/// </summary>
public sealed class PaymentByNumberNotFoundException(string paymentNumber) : NotFoundException($"payment with number {paymentNumber} not found");

/// <summary>
/// Exception thrown when trying to create a payment with a duplicate number.
/// </summary>
public sealed class DuplicatePaymentNumberException(string paymentNumber) : ConflictException($"payment with number {paymentNumber} already exists");

/// <summary>
/// Exception thrown when a payment amount is invalid (negative or zero).
/// </summary>
public sealed class InvalidPaymentAmountException() : ForbiddenException("payment amount must be positive");

/// <summary>
/// Exception thrown when trying to modify a processed payment.
/// </summary>
public sealed class CannotModifyProcessedPaymentException(DefaultIdType id) : ForbiddenException($"cannot modify processed payment with id {id}");

/// <summary>
/// Exception thrown when the payment date is invalid.
/// </summary>
public sealed class InvalidPaymentDateException() : ForbiddenException("payment date cannot be in the future");

/// <summary>
/// Exception thrown when trying to void an already voided payment.
/// </summary>
public sealed class PaymentAlreadyVoidedException(DefaultIdType id) : ForbiddenException($"payment with id {id} is already voided");

/// <summary>
/// Exception thrown when payment method is not supported.
/// </summary>
public sealed class UnsupportedPaymentMethodException(string paymentMethod) : ForbiddenException($"payment method '{paymentMethod}' is not supported");
