namespace Store.Domain.Exceptions.PosPayment;

/// <summary>
/// Exception thrown when ID does not find a POS payment.
/// </summary>
public sealed class PosPaymentByIdNotFoundException(DefaultIdType id) : NotFoundException($"POS payment with id {id} not found");

/// <summary>
/// Exception thrown when the POS payment amount is invalid.
/// </summary>
public sealed class InvalidPosPaymentAmountException() : ForbiddenException("POS payment amount must be positive");

/// <summary>
/// Exception thrown when the payment method is not supported.
/// </summary>
public sealed class UnsupportedPaymentMethodException(string paymentMethod) : ForbiddenException($"payment method '{paymentMethod}' is not supported");

/// <summary>
/// Exception thrown when trying to modify a processed POS payment.
/// </summary>
public sealed class CannotModifyProcessedPosPaymentException(DefaultIdType id) : ForbiddenException($"cannot modify processed POS payment with id {id}");

/// <summary>
/// Exception thrown when trying to void an already voided POS payment.
/// </summary>
public sealed class PosPaymentAlreadyVoidedException(DefaultIdType id) : ForbiddenException($"POS payment with id {id} is already voided");

/// <summary>
/// Exception thrown when refund amount exceeds original payment amount.
/// </summary>
public sealed class RefundExceedsPaymentAmountException(decimal paymentAmount, decimal refundAmount) 
    : ForbiddenException($"refund amount {refundAmount:C} exceeds payment amount {paymentAmount:C}");

/// <summary>
/// Exception thrown when the payment date is invalid.
/// </summary>
public sealed class InvalidPaymentDateException() : ForbiddenException("payment date cannot be in the future");
