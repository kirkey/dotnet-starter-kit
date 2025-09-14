namespace Accounting.Application.Budgets.BudgetLines.Exceptions;

public class BudgetLineAlreadyExistsException : Exception
{
    public BudgetLineAlreadyExistsException(DefaultIdType budgetId, DefaultIdType accountId)
        : base($"Budget line for account {accountId} in budget {budgetId} already exists.") { }
}

public class BudgetLineNotFoundException : Exception
{
    public BudgetLineNotFoundException(DefaultIdType budgetId, DefaultIdType accountId)
        : base($"Budget line for account {accountId} in budget {budgetId} was not found.") { }
}

