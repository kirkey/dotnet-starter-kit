// Budget Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class BudgetNotFoundException(DefaultIdType id) : NotFoundException($"budget with id {id} not found");
public sealed class BudgetAlreadyApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is already approved");
public sealed class BudgetNotApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is not approved");
public sealed class BudgetCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"budget with id {id} cannot be modified after approval");
public sealed class BudgetLineNotFoundException(DefaultIdType budgetId, DefaultIdType accountId) : NotFoundException($"budget line for account {accountId} in budget {budgetId} not found");
public sealed class BudgetLineAlreadyExistsException(DefaultIdType budgetId, DefaultIdType accountId) : ForbiddenException($"budget line for account {accountId} already exists in budget {budgetId}");
public sealed class InvalidBudgetAmountException() : ForbiddenException("budget amount cannot be negative");
public sealed class EmptyBudgetCannotBeApprovedException(DefaultIdType id) : ForbiddenException($"cannot approve budget {id} with no budget lines");