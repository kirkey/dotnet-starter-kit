using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.ConsumptionData;

public record ConsumptionDataCreated(DefaultIdType ConsumptionId, DefaultIdType MeterId, DateTime ReadingDate, decimal KWhUsed, string BillingPeriod, string? Description, string? Notes) : DomainEvent;

public record ConsumptionDataUpdated(Accounting.Domain.ConsumptionData ConsumptionData) : DomainEvent;

public record ConsumptionDataDeleted(DefaultIdType ConsumptionId) : DomainEvent;

public record ConsumptionDataMarkedAsEstimated(DefaultIdType ConsumptionId, DefaultIdType MeterId, string Reason) : DomainEvent;
