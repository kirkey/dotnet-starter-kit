using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.AccountingPeriods.Exceptions;

public class AccountingPeriodNotFoundException : NotFoundException
{
    public AccountingPeriodNotFoundException(DefaultIdType id) : base($"Accounting Period with ID {id} was not found.")
    {
    }
}

public class AccountingPeriodAlreadyExistsException : ConflictException
{
    public AccountingPeriodAlreadyExistsException(int fiscalYear, string periodType) : base($"Accounting Period for fiscal year {fiscalYear} with type '{periodType}' already exists.")
    {
    }
}

public class InvalidAccountingPeriodDateRangeException : BadRequestException
{
    public InvalidAccountingPeriodDateRangeException() : base("Start date must be before end date.")
    {
    }
}
