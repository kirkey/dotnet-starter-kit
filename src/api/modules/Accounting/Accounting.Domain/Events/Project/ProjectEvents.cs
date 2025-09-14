namespace Accounting.Domain.Events.Project;

public record ProjectCreated(DefaultIdType Id, string ProjectName, DateTime StartDate, decimal BudgetedAmount, string? Description, string? Notes) : DomainEvent;

public record ProjectUpdated(Accounting.Domain.Project Project) : DomainEvent;

public record ProjectDeleted(DefaultIdType Id) : DomainEvent;

public record ProjectCostAdded(DefaultIdType ProjectId, decimal Amount, decimal TotalActualCost) : DomainEvent;

public record ProjectRevenueAdded(DefaultIdType ProjectId, decimal Amount, decimal TotalActualRevenue) : DomainEvent;

public record ProjectCompleted(DefaultIdType ProjectId, DateTime CompletionDate, decimal ActualCost, decimal ActualRevenue) : DomainEvent;

public record ProjectCancelled(DefaultIdType ProjectId, DateTime CancellationDate, string Reason) : DomainEvent;
