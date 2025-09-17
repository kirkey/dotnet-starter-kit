namespace Accounting.Application.Budgets.Exceptions;

public class BudgetCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Budget with ID {id} cannot be modified.");

public class InvalidBudgetAmountException() : ForbiddenException("Budget amount must be greater than zero.");

public class BudgetAlreadyExistsException(string name, DefaultIdType periodId)
    : Exception($"A budget named '{name}' already exists for period {periodId}.");

public class BudgetCannotBeDeletedException(DefaultIdType id)
    : Exception($"Budget with ID {id} cannot be deleted (status prevents deletion).");
