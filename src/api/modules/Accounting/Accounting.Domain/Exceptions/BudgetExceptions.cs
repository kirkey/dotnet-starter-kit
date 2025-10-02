// Budget Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class BudgetNotFoundException(DefaultIdType id) : NotFoundException($"budget with id {id} not found");
public sealed class BudgetAlreadyApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is already approved");
public sealed class BudgetNotApprovedException(DefaultIdType id) : ForbiddenException($"budget with id {id} is not approved");
public sealed class BudgetCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"budget with id {id} cannot be modified after approval");
public sealed class InvalidBudgetAmountException() : ForbiddenException("budget amount cannot be negative");
public sealed class EmptyBudgetCannotBeApprovedException(DefaultIdType id) : ForbiddenException($"cannot approve budget {id} with no budget lines");

// Domain-level exceptions matching the renamed BudgetDetail type (aliases for compatibility)
public sealed class BudgetDetailNotFoundException(DefaultIdType budgetId, DefaultIdType accountId)
    : NotFoundException($"budget detail for account {accountId} in budget {budgetId} not found");

public sealed class BudgetDetailAlreadyExistsException(DefaultIdType budgetId, DefaultIdType accountId)
    : ForbiddenException($"budget detail for account {accountId} already exists in budget {budgetId}");
