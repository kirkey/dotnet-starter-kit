using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Budgets.Exceptions;

public class BudgetCannotBeModifiedException : ForbiddenException
{
    public BudgetCannotBeModifiedException(DefaultIdType id) : base($"Budget with ID {id} cannot be modified.")
    {
    }
}

public class InvalidBudgetAmountException : ForbiddenException
{
    public InvalidBudgetAmountException() : base("Budget amount must be greater than zero.")
    {
    }
}
