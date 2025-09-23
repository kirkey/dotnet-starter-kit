// Project Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class ProjectNotFoundException(DefaultIdType id) : NotFoundException($"project with id {id} not found");
public sealed class ProjectAlreadyCompletedException(DefaultIdType id) : ForbiddenException($"project with id {id} is already completed");
public sealed class ProjectAlreadyCancelledException(DefaultIdType id) : ForbiddenException($"project with id {id} is already cancelled");
public sealed class ProjectCannotBeModifiedException(DefaultIdType id) : ForbiddenException($"project with id {id} cannot be modified after completion or cancellation");
public sealed class InvalidProjectBudgetException() : ForbiddenException("project budget amount cannot be negative");
public sealed class InvalidProjectCostEntryException() : ForbiddenException("project cost entry amount must be positive");
public sealed class InvalidProjectRevenueEntryException() : ForbiddenException("project revenue entry amount must be positive");
public sealed class JobCostingEntryNotFoundException(DefaultIdType id) : NotFoundException($"job costing entry with id {id} not found");

/// <summary>
/// Exception thrown when a provided project end date is before the project start date.
/// </summary>
public sealed class InvalidProjectEndDateException() : ForbiddenException("project end date cannot be before start date");

/// <summary>
/// Exception thrown when attempting to mark a project as Completed without an end date.
/// </summary>
public sealed class ProjectCompletionRequiresEndDateException() : ForbiddenException("marking a project as Completed requires an end date to be set");

/// <summary>
/// Exception thrown when adding a cost would push actual cost beyond the approved budget.
/// </summary>
public sealed class ProjectBudgetExceededException(decimal attemptedAmount, decimal budgetedAmount, decimal currentActualCost)
    : ForbiddenException($"adding amount {attemptedAmount:C} would exceed project budget {budgetedAmount:C} (current actual cost {currentActualCost:C})");
