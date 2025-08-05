using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.AccountingPeriod;

public record AccountingPeriodCreated(DefaultIdType Id, string PeriodName, DateTime StartDate, DateTime EndDate, int FiscalYear, string? Description, string? Notes) : DomainEvent;

public record AccountingPeriodUpdated(Accounting.Domain.AccountingPeriod Period) : DomainEvent;

public record AccountingPeriodClosed(DefaultIdType Id, string PeriodName, DateTime EndDate) : DomainEvent;

public record AccountingPeriodReopened(DefaultIdType Id, string PeriodName) : DomainEvent;
