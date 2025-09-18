namespace Accounting.Domain.Events.BillingCycle;

public record BillingCycleCreated(DefaultIdType Id, string CycleName, DateTime StartDate, DateTime EndDate) : DomainEvent;

public record BillingCycleUpdated(DefaultIdType Id, string? CycleName) : DomainEvent;

public record BillingCycleDeleted(DefaultIdType Id) : DomainEvent;
