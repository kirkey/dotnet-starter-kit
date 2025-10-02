namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a credit memo is not found.
/// </summary>
public sealed class CreditMemoNotFoundException(DefaultIdType id)
    : NotFoundException($"credit memo with id {id} not found");

/// <summary>
/// Exception thrown when a credit memo cannot be modified due to its current state.
/// </summary>
public sealed class CreditMemoCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"credit memo with id {id} cannot be modified");

/// <summary>
/// Exception thrown when a credit memo is already approved.
/// </summary>
public sealed class CreditMemoAlreadyApprovedException(DefaultIdType id)
    : ForbiddenException($"credit memo with id {id} is already approved");

/// <summary>
/// Exception thrown when a credit memo is already voided.
/// </summary>
public sealed class CreditMemoAlreadyVoidedException(DefaultIdType id)
    : ForbiddenException($"credit memo with id {id} is already voided");

/// <summary>
/// Exception thrown when trying to apply or refund an unapproved credit memo.
/// </summary>
public sealed class CreditMemoNotApprovedException(DefaultIdType id)
    : ForbiddenException($"credit memo with id {id} must be approved before application or refund");

/// <summary>
/// Exception thrown when the amount to apply or refund exceeds the unapplied balance.
/// </summary>
public sealed class CreditMemoInsufficientBalanceException(DefaultIdType id, decimal requested, decimal available)
    : ForbiddenException($"credit memo with id {id} has insufficient balance. Requested: {requested:C}, Available: {available:C}");

/// <summary>
/// Exception thrown when the credit memo amount is invalid.
/// </summary>
public sealed class InvalidCreditMemoAmountException()
    : BadRequestException("credit memo amount must be positive");

/// <summary>
/// Exception thrown when the credit memo reference type is invalid.
/// </summary>
public sealed class InvalidCreditMemoReferenceTypeException(string referenceType)
    : BadRequestException($"invalid credit memo reference type: {referenceType}. Must be 'Customer' or 'Vendor'");
