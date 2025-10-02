using Accounting.Domain.Entities;

namespace Accounting.Domain.Events.Consumptions;

public record ConsumptionCreated(DefaultIdType ConsumptionId, DefaultIdType MeterId, DateTime ReadingDate, decimal KWhUsed, string BillingPeriod, string? Description, string? Notes) : DomainEvent;

public record ConsumptionUpdated(Consumption Consumption) : DomainEvent;

public record ConsumptionDeleted(DefaultIdType ConsumptionId) : DomainEvent;

public record ConsumptionMarkedAsEstimated(DefaultIdType ConsumptionId, DefaultIdType MeterId, string Reason) : DomainEvent;
