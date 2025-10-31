// Retained Earnings Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when retained earnings is not found by ID.
/// </summary>
public sealed class RetainedEarningsByIdNotFoundException(DefaultIdType id) : NotFoundException($"retained earnings with id {id} not found");

/// <summary>
/// Exception thrown when retained earnings is not found by fiscal year.
/// </summary>
public sealed class RetainedEarningsByFiscalYearNotFoundException(int fiscalYear) : NotFoundException($"retained earnings for fiscal year {fiscalYear} not found");

/// <summary>
/// Exception thrown when trying to create retained earnings for a duplicate fiscal year.
/// </summary>
public sealed class DuplicateFiscalYearException(int fiscalYear) : ConflictException($"retained earnings for fiscal year {fiscalYear} already exists");

/// <summary>
/// Exception thrown when fiscal year is out of valid range.
/// </summary>
public sealed class InvalidFiscalYearException(int minYear, int maxYear) : ForbiddenException($"fiscal year must be between {minYear} and {maxYear}");

/// <summary>
/// Exception thrown when fiscal year end date is not after start date.
/// </summary>
public sealed class InvalidFiscalYearDatesException() : ForbiddenException("fiscal year end date must be after start date");

/// <summary>
/// Exception thrown when trying to modify closed retained earnings.
/// </summary>
public sealed class CannotModifyClosedRetainedEarningsException(int fiscalYear) : ForbiddenException($"cannot modify closed retained earnings for fiscal year {fiscalYear}");

/// <summary>
/// Exception thrown when distribution amount is invalid (non-positive).
/// </summary>
public sealed class InvalidDistributionAmountException() : ForbiddenException("distribution amount must be positive");

/// <summary>
/// Exception thrown when capital contribution amount is invalid (non-positive).
/// </summary>
public sealed class InvalidCapitalContributionAmountException() : ForbiddenException("capital contribution amount must be positive");

/// <summary>
/// Exception thrown when appropriation amount exceeds unappropriated balance.
/// </summary>
public sealed class AppropriationExceedsBalanceException(decimal amount, decimal available) : ForbiddenException($"appropriation amount {amount:C} exceeds unappropriated balance {available:C}");

/// <summary>
/// Exception thrown when release amount exceeds appropriated balance.
/// </summary>
public sealed class ReleaseExceedsAppropriatedBalanceException(decimal amount, decimal appropriated) : ForbiddenException($"release amount {amount:C} exceeds appropriated balance {appropriated:C}");

/// <summary>
/// Exception thrown when trying to close already closed retained earnings.
/// </summary>
public sealed class RetainedEarningsAlreadyClosedException(int fiscalYear) : ForbiddenException($"retained earnings for fiscal year {fiscalYear} is already closed");

/// <summary>
/// Exception thrown when trying to reopen not-closed retained earnings.
/// </summary>
public sealed class RetainedEarningsNotClosedException(int fiscalYear) : ForbiddenException($"retained earnings for fiscal year {fiscalYear} is not closed");

/// <summary>
/// Exception thrown when distributions exceed available retained earnings.
/// </summary>
public sealed class DistributionExceedsRetainedEarningsException(decimal distribution, decimal available) : ForbiddenException($"distribution amount {distribution:C} exceeds available retained earnings {available:C}");

/// <summary>
/// Exception thrown when closing balance calculation results in invalid amount.
/// </summary>
public sealed class InvalidClosingBalanceException() : ForbiddenException("calculated closing balance is invalid");

