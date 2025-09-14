namespace Accounting.Domain.Events.RateSchedule;

public record RateScheduleCreated(DefaultIdType Id, string RateCode, string RateName, DateTime EffectiveDate, string? Description) : DomainEvent;
public record RateScheduleUpdated(Accounting.Domain.RateSchedule RateSchedule) : DomainEvent;
public record RateScheduleDeleted(DefaultIdType Id) : DomainEvent;
public record RateTierAdded(DefaultIdType RateScheduleId, DefaultIdType RateTierId, int TierOrder, decimal UpToKwh, decimal RatePerKwh) : DomainEvent;
