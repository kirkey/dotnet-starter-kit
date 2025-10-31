// Prepaid Expense Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a prepaid expense is not found by ID.
/// </summary>
public sealed class PrepaidExpenseByIdNotFoundException(DefaultIdType id) : NotFoundException($"prepaid expense with id {id} not found");

/// <summary>
/// Exception thrown when a prepaid expense is not found by prepaid number.
/// </summary>
public sealed class PrepaidExpenseByNumberNotFoundException(string prepaidNumber) : NotFoundException($"prepaid expense with number {prepaidNumber} not found");

/// <summary>
/// Exception thrown when trying to create a prepaid expense with a duplicate number.
/// </summary>
public sealed class DuplicatePrepaidExpenseNumberException(string prepaidNumber) : ConflictException($"prepaid expense with number {prepaidNumber} already exists");

/// <summary>
/// Exception thrown when total amount is invalid (non-positive).
/// </summary>
public sealed class InvalidPrepaidAmountException() : ForbiddenException("prepaid expense total amount must be positive");

/// <summary>
/// Exception thrown when end date is not after start date.
/// </summary>
public sealed class InvalidPrepaidPeriodException() : ForbiddenException("prepaid expense end date must be after start date");

/// <summary>
/// Exception thrown when trying to modify a fully amortized prepaid expense.
/// </summary>
public sealed class CannotModifyFullyAmortizedPrepaidException(DefaultIdType id) : ForbiddenException($"cannot modify fully amortized prepaid expense with id {id}");

/// <summary>
/// Exception thrown when amortization amount exceeds remaining balance.
/// </summary>
public sealed class AmortizationExceedsRemainingBalanceException(decimal amortizationAmount, decimal remainingBalance) 
    : ForbiddenException($"amortization amount {amortizationAmount:C} exceeds remaining balance {remainingBalance:C}");

/// <summary>
/// Exception thrown when trying to amortize a fully amortized prepaid.
/// </summary>
public sealed class PrepaidAlreadyFullyAmortizedException(DefaultIdType id) : ForbiddenException($"prepaid expense with id {id} is already fully amortized");

/// <summary>
/// Exception thrown when amortization amount is not positive.
/// </summary>
public sealed class InvalidAmortizationAmountException() : ForbiddenException("amortization amount must be positive");

/// <summary>
/// Exception thrown when trying to close a prepaid that is not fully amortized.
/// </summary>
public sealed class CannotClosePartiallyAmortizedPrepaidException(DefaultIdType id) : ForbiddenException($"can only close fully amortized prepaid expenses");

/// <summary>
/// Exception thrown when trying to cancel a prepaid with amortization history.
/// </summary>
public sealed class CannotCancelAmortizedPrepaidException(DefaultIdType id) : ForbiddenException($"cannot cancel prepaid expense {id} with amortization history");

/// <summary>
/// Exception thrown when amortization schedule is invalid.
/// </summary>
public sealed class InvalidAmortizationScheduleException(string schedule) : ForbiddenException($"invalid amortization schedule: {schedule}");

