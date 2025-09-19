namespace Accounting.Application.AccountingPeriods.Exceptions;

/// <summary>
/// Exception types used by the AccountingPeriods application layer to represent common error conditions.
/// </summary>
public class AccountingPeriodNotFoundException(DefaultIdType id)
    : NotFoundException($"Accounting Period with ID {id} was not found.");

/// <summary>
/// Thrown when trying to create a period for a fiscal year/period type combination that already exists.
/// </summary>
public class AccountingPeriodAlreadyExistsException(int fiscalYear, string periodType)
    : ConflictException($"Accounting Period for fiscal year {fiscalYear} with type '{periodType}' already exists.");

/// <summary>
/// Thrown when a period name conflicts with an existing period.
/// </summary>
public class AccountingPeriodNameAlreadyExistsException(string name)
    : ConflictException($"Accounting Period with name '{name}' already exists.");

/// <summary>
/// Thrown when a new or updated period overlaps an existing period's date range.
/// </summary>
public class AccountingPeriodOverlappingException(DateTime startDate, DateTime endDate)
    : ConflictException($"Accounting Period overlaps with existing period in range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}.");

/// <summary>
/// Thrown when the provided start/end date range is invalid.
/// </summary>
public class InvalidAccountingPeriodDateRangeException() : BadRequestException("Start date must be before end date.");
