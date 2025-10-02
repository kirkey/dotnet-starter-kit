namespace Accounting.Domain.Events.Budget;

public record BudgetCreated(DefaultIdType Id, string BudgetName, DefaultIdType PeriodId, string PeriodName, int FiscalYear, string BudgetType, string? Description, string? Notes) : DomainEvent;

public record BudgetUpdated(Entities.Budget Budget) : DomainEvent;

public record BudgetDeleted(DefaultIdType Id) : DomainEvent;

public record BudgetDetailAdded(DefaultIdType BudgetId, DefaultIdType AccountId, decimal BudgetedAmount) : DomainEvent;

public record BudgetDetailUpdated(DefaultIdType BudgetId, DefaultIdType AccountId, decimal BudgetedAmount) : DomainEvent;

public record BudgetDetailRemoved(DefaultIdType BudgetId, DefaultIdType AccountId) : DomainEvent;

public record BudgetApproved(DefaultIdType Id, DateTime ApprovedDate, string ApprovedBy) : DomainEvent;

public record BudgetActivated(DefaultIdType Id, string BudgetName) : DomainEvent;

public record BudgetClosed(DefaultIdType Id, string BudgetName) : DomainEvent;
