// Deferred Revenue Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when deferred revenue is not found by ID.
/// </summary>
public sealed class DeferredRevenueByIdNotFoundException(DefaultIdType id) : NotFoundException($"deferred revenue with id {id} not found");

/// <summary>
/// Exception thrown when deferred revenue is not found by number.
/// </summary>
public sealed class DeferredRevenueByNumberNotFoundException(string deferredRevenueNumber) : NotFoundException($"deferred revenue with number {deferredRevenueNumber} not found");

/// <summary>
/// Exception thrown when trying to create deferred revenue with a duplicate number.
/// </summary>
public sealed class DuplicateDeferredRevenueNumberException(string deferredRevenueNumber) : ConflictException($"deferred revenue with number {deferredRevenueNumber} already exists");

/// <summary>
/// Exception thrown when deferred revenue amount is invalid (negative or zero).
/// </summary>
public sealed class InvalidDeferredRevenueAmountException() : ForbiddenException("deferred revenue amount must be positive");

/// <summary>
/// Exception thrown when trying to recognize more than the available deferred amount.
/// </summary>
public sealed class InsufficientDeferredRevenueException(decimal availableAmount, decimal requestedAmount) 
    : ForbiddenException($"cannot recognize {requestedAmount:N2}. Only {availableAmount:N2} available");

/// <summary>
/// Exception thrown when trying to modify fully recognized deferred revenue.
/// </summary>
public sealed class CannotModifyRecognizedDeferredRevenueException(DefaultIdType id) : ForbiddenException($"cannot modify fully recognized deferred revenue with id {id}");

/// <summary>
/// Exception thrown when the recognition date is invalid.
/// </summary>
public sealed class InvalidRecognitionDateException() : ForbiddenException("recognition date cannot be in the past for new deferrals");

/// <summary>
/// Exception thrown when trying to recognize revenue before the recognition date.
/// </summary>
public sealed class PrematureRevenueRecognitionException(DateTime recognitionDate) : ForbiddenException($"cannot recognize revenue before {recognitionDate:yyyy-MM-dd}");
