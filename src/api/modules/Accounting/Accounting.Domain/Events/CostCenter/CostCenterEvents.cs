using Accounting.Domain.Entities;

namespace Accounting.Domain.Events.CostCenter;

public sealed record CostCenterCreated(
    DefaultIdType Id,
    string Code,
    string Name,
    CostCenterType CostCenterType) : DomainEvent;

public sealed record CostCenterUpdated(
    DefaultIdType Id,
    string Name) : DomainEvent;

public sealed record CostCenterBudgetUpdated(
    DefaultIdType Id,
    decimal BudgetAmount) : DomainEvent;

public sealed record CostCenterActualRecorded(
    DefaultIdType Id,
    decimal Amount,
    decimal TotalActual) : DomainEvent;

public sealed record CostCenterActivated(DefaultIdType Id) : DomainEvent;

public sealed record CostCenterDeactivated(DefaultIdType Id) : DomainEvent;

public sealed record CostCenterDeleted(DefaultIdType Id) : DomainEvent;
