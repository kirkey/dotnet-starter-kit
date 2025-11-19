using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when shift is created.
/// </summary>
public record ShiftCreated : DomainEvent
{
    public Shift Shift { get; init; } = default!;
}

/// <summary>
/// Event raised when shift is updated.
/// </summary>
public record ShiftUpdated : DomainEvent
{
    public Shift Shift { get; init; } = default!;
}

/// <summary>
/// Event raised when break is added to shift.
/// </summary>
public record ShiftBreakAdded : DomainEvent
{
    public Shift Shift { get; init; } = default!;
    public ShiftBreak Break { get; init; } = default!;
}

/// <summary>
/// Event raised when break is removed from shift.
/// </summary>
public record ShiftBreakRemoved : DomainEvent
{
    public Shift Shift { get; init; } = default!;
    public DefaultIdType BreakId { get; init; }
}

/// <summary>
/// Event raised when shift is deactivated.
/// </summary>
public record ShiftDeactivated : DomainEvent
{
    public DefaultIdType ShiftId { get; init; }
}

/// <summary>
/// Event raised when shift is activated.
/// </summary>
public record ShiftActivated : DomainEvent
{
    public DefaultIdType ShiftId { get; init; }
}

/// <summary>
/// Event raised when shift assignment is created.
/// </summary>
public record ShiftAssignmentCreated : DomainEvent
{
    public ShiftAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when shift assignment is updated.
/// </summary>
public record ShiftAssignmentUpdated : DomainEvent
{
    public ShiftAssignment Assignment { get; init; } = default!;
}

/// <summary>
/// Event raised when shift assignment is deactivated.
/// </summary>
public record ShiftAssignmentDeactivated : DomainEvent
{
    public DefaultIdType AssignmentId { get; init; }
}

