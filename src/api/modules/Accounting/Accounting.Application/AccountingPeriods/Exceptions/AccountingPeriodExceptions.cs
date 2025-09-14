namespace Accounting.Application.AccountingPeriods.Exceptions;

public class AccountingPeriodNotFoundException(DefaultIdType id)
    : NotFoundException($"Accounting Period with ID {id} was not found.");

public class AccountingPeriodAlreadyExistsException(int fiscalYear, string periodType)
    : ConflictException($"Accounting Period for fiscal year {fiscalYear} with type '{periodType}' already exists.");

public class AccountingPeriodNameAlreadyExistsException(string name)
    : ConflictException($"Accounting Period with name '{name}' already exists.");

public class AccountingPeriodOverlappingException(DateTime startDate, DateTime endDate)
    : ConflictException($"Accounting Period overlaps with existing period in range {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}.");

public class InvalidAccountingPeriodDateRangeException() : BadRequestException("Start date must be before end date.");
