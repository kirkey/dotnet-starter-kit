namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a debit memo is not found.
/// </summary>
public sealed class DebitMemoNotFoundException(DefaultIdType id)
    : NotFoundException($"debit memo with id {id} not found");

/// <summary>
/// Exception thrown when a debit memo cannot be modified due to its current state.
/// </summary>
public sealed class DebitMemoCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"debit memo with id {id} cannot be modified");

/// <summary>
/// Exception thrown when a debit memo is already approved.
/// </summary>
public sealed class DebitMemoAlreadyApprovedException(DefaultIdType id)
    : ForbiddenException($"debit memo with id {id} is already approved");

/// <summary>
/// Exception thrown when a debit memo is already voided.
/// </summary>
public sealed class DebitMemoAlreadyVoidedException(DefaultIdType id)
    : ForbiddenException($"debit memo with id {id} is already voided");

/// <summary>
/// Exception thrown when trying to apply an unapproved debit memo.
/// </summary>
public sealed class DebitMemoNotApprovedException(DefaultIdType id)
    : ForbiddenException($"debit memo with id {id} must be approved before application");

/// <summary>
/// Exception thrown when the amount to apply exceeds the unapplied balance.
/// </summary>
public sealed class DebitMemoInsufficientBalanceException(DefaultIdType id, decimal requested, decimal available)
    : ForbiddenException($"debit memo with id {id} has insufficient balance. Requested: {requested:C}, Available: {available:C}");

/// <summary>
/// Exception thrown when the debit memo amount is invalid.
/// </summary>
public sealed class InvalidDebitMemoAmountException()
    : BadRequestException("debit memo amount must be positive");

/// <summary>
/// Exception thrown when the debit memo reference type is invalid.
/// </summary>
public sealed class InvalidDebitMemoReferenceTypeException(string referenceType)
    : BadRequestException($"invalid debit memo reference type: {referenceType}. Must be 'Customer' or 'Vendor'");
