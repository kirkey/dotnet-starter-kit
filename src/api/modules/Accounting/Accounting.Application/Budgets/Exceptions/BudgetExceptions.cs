using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Budgets.Exceptions;

public class BudgetCannotBeModifiedException(DefaultIdType id)
    : ForbiddenException($"Budget with ID {id} cannot be modified.");

public class InvalidBudgetAmountException() : ForbiddenException("Budget amount must be greater than zero.");
