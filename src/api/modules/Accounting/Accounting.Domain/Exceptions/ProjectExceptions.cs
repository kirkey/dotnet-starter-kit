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