using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when timesheet is created.
/// </summary>
public record TimesheetCreated : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
}

/// <summary>
/// Event raised when a line is added to timesheet.
/// </summary>
public record TimesheetLineAdded : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
    public TimesheetLine Line { get; init; } = default!;
}

/// <summary>
/// Event raised when a line is removed from timesheet.
/// </summary>
public record TimesheetLineRemoved : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
    public DefaultIdType LineId { get; init; }
}

/// <summary>
/// Event raised when timesheet is submitted.
/// </summary>
public record TimesheetSubmitted : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
}

/// <summary>
/// Event raised when timesheet is approved.
/// </summary>
public record TimesheetApproved : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
}

/// <summary>
/// Event raised when timesheet is rejected.
/// </summary>
public record TimesheetRejected : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
}

/// <summary>
/// Event raised when timesheet is locked.
/// </summary>
public record TimesheetLocked : DomainEvent
{
    public DefaultIdType TimesheetId { get; init; }
}

/// <summary>
/// Event raised when timesheet is unlocked.
/// </summary>
public record TimesheetUnlocked : DomainEvent
{
    public DefaultIdType TimesheetId { get; init; }
}

/// <summary>
/// Event raised when timesheet is reset to draft.
/// </summary>
public record TimesheetResetToDraft : DomainEvent
{
    public Timesheet Timesheet { get; init; } = default!;
}

