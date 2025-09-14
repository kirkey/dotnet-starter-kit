namespace Accounting.Application.Budgets.Exceptions;

public class BudgetCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Budget with ID {id} cannot be modified.");

public class InvalidBudgetAmountException() : ForbiddenException("Budget amount must be greater than zero.");

public class BudgetAlreadyExistsException : Exception
{
    public BudgetAlreadyExistsException(string name, DefaultIdType periodId)
        : base($"A budget named '{name}' already exists for period {periodId}.") { }
}

public class BudgetCannotBeDeletedException : Exception
{
    public BudgetCannotBeDeletedException(DefaultIdType id)
        : base($"Budget with ID {id} cannot be deleted (status prevents deletion).") { }
}
