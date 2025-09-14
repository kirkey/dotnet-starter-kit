namespace Accounting.Application.Accruals.Exceptions;

public class AccrualAlreadyExistsException : Exception
{
    public AccrualAlreadyExistsException(string accrualNumber)
        : base($"An accrual with number '{accrualNumber}' already exists.")
    {
    }
}

public class AccrualNotFoundException : Exception
{
    public AccrualNotFoundException(DefaultIdType id)
        : base($"Accrual with Id {id} was not found.")
    {
    }
}

public class AccrualCannotBeDeletedException : Exception
{
    public AccrualCannotBeDeletedException(DefaultIdType id)
        : base($"Accrual with Id {id} cannot be deleted.")
    {
    }
}
