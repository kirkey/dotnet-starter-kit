namespace Accounting.Application.Accruals.Exceptions;

public class AccrualAlreadyExistsException(string accrualNumber)
    : Exception($"An accrual with number '{accrualNumber}' already exists.");

public class AccrualNotFoundException(DefaultIdType id) : Exception($"Accrual with Id {id} was not found.");

public class AccrualCannotBeDeletedException(DefaultIdType id) : Exception($"Accrual with Id {id} cannot be deleted.");
