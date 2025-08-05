using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Budget;

public record BudgetCreated(DefaultIdType Id, string BudgetName, DefaultIdType PeriodId, int FiscalYear, string BudgetType, string? Description, string? Notes) : DomainEvent;

public record BudgetUpdated(Accounting.Domain.Budget Budget) : DomainEvent;

public record BudgetLineAdded(DefaultIdType BudgetId, DefaultIdType AccountId, decimal BudgetedAmount) : DomainEvent;

public record BudgetLineUpdated(DefaultIdType BudgetId, DefaultIdType AccountId, decimal BudgetedAmount) : DomainEvent;

public record BudgetLineRemoved(DefaultIdType BudgetId, DefaultIdType AccountId) : DomainEvent;

public record BudgetApproved(DefaultIdType Id, DateTime ApprovedDate, string ApprovedBy) : DomainEvent;

public record BudgetActivated(DefaultIdType Id, string BudgetName) : DomainEvent;

public record BudgetClosed(DefaultIdType Id, string BudgetName) : DomainEvent;
