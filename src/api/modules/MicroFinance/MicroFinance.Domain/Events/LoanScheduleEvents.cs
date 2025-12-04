using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a loan schedule entry is created.</summary>
public sealed record LoanScheduleCreated : DomainEvent
{
    public LoanSchedule? LoanSchedule { get; init; }
}

/// <summary>Domain event raised when a loan schedule is paid.</summary>
public sealed record LoanSchedulePaid : DomainEvent
{
    public DefaultIdType ScheduleId { get; init; }
}

/// <summary>Domain event raised when a loan schedule is waived.</summary>
public sealed record LoanScheduleWaived : DomainEvent
{
    public DefaultIdType ScheduleId { get; init; }
}
