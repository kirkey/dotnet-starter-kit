namespace Accounting.Application.Budgets.BudgetLines.Exceptions;

public class BudgetLineAlreadyExistsException(DefaultIdType budgetId, DefaultIdType accountId)
    : Exception($"Budget line for account {accountId} in budget {budgetId} already exists.");

public class BudgetLineNotFoundException(DefaultIdType budgetId, DefaultIdType accountId)
    : Exception($"Budget line for account {accountId} in budget {budgetId} was not found.");

