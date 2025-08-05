using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Meter;

public record MeterCreated(
    DefaultIdType Id, string MeterNumber, string MeterType, string Manufacturer, string ModelNumber, string? Description, string? Notes) : DomainEvent;

public record MeterUpdated(
    DefaultIdType Id, string MeterNumber, string MeterType, string? Description, string? Notes) : DomainEvent;

public record MeterStatusChanged(
    DefaultIdType Id, string MeterNumber, string Status) : DomainEvent;

public record MeterMaintenanceUpdated(
    DefaultIdType Id, string MeterNumber, DateTime? LastMaintenanceDate, DateTime? NextCalibrationDate) : DomainEvent;

public record MeterReadingAdded(
    DefaultIdType Id, string MeterNumber, decimal Reading, DateTime ReadingDate, string ReadingType) : DomainEvent;
