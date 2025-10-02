// AccountingPeriodExceptions.cs
// Per-domain exceptions for Accounting Periods

namespace Accounting.Domain.Exceptions;

public sealed class AccountingPeriodNotFoundException(DefaultIdType id) : NotFoundException($"accounting period with id {id} not found");
public sealed class AccountingPeriodAlreadyClosedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} is already closed");
public sealed class AccountingPeriodNotClosedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} is not closed");
public sealed class AccountingPeriodCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"accounting period with id {id} cannot be modified after closing");
public sealed class InvalidAccountingPeriodDateRangeException() : ForbiddenException("accounting period start date must be before end date");
// New domain validation exceptions for AccountingPeriod
public sealed class AccountingPeriodInvalidNameException(string message) : ForbiddenException(message);
public sealed class AccountingPeriodInvalidPeriodTypeException(string periodType) : ForbiddenException($"invalid accounting period type '{periodType}'");
public sealed class AccountingPeriodInvalidFiscalYearException(int year) : ForbiddenException($"invalid fiscal year '{year}'");