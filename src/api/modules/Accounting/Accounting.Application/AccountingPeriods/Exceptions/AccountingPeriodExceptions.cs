using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.AccountingPeriods.Exceptions;

public class AccountingPeriodNotFoundException(DefaultIdType id)
    : NotFoundException($"Accounting Period with ID {id} was not found.");

public class AccountingPeriodAlreadyExistsException(int fiscalYear, string periodType)
    : ConflictException($"Accounting Period for fiscal year {fiscalYear} with type '{periodType}' already exists.");

public class InvalidAccountingPeriodDateRangeException() : BadRequestException("Start date must be before end date.");
